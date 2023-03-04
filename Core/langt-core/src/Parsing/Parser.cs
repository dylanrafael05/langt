using System;
using Langt.AST;
using Langt.Structure;
using Langt.Lexing;
using System.Runtime.CompilerServices;
using TT = Langt.Lexing.TokenType;
using Langt.Structure.Collections;

namespace Langt.Parsing;

public delegate ASTNode ParsePass(ParserState state);
public delegate T ParsePass<T>(ParserState state) where T : ASTNode;

// TODO: handle (trivia) properly!
public sealed class Parser : LookaheadListStream<Token>, IProjectDependency
{
    public LangtProject Project {get; init;}
    public string CurrentTypeName => CurrentType?.GetReadableName() ?? "null token";

    public static readonly ParseOperatorSpec[] Operators = new ParseOperatorSpec[] 
    {
        new(ParseOperatorType.UnaryPrefix    , TT.Ampersand, TT.Star),
        new(ParseOperatorType.UnaryPrefix    , TT.Not),
        new(ParseOperatorType.LeftRecursive  , TT.Or),
        new(ParseOperatorType.LeftRecursive  , TT.And),
        new(ParseOperatorType.LeftRecursive  , TT.DoubleEquals, TT.GreaterThan, TT.GreaterEqual, TT.LessEqual, TT.LessThan, TT.NotEquals),
        new(ParseOperatorType.LeftRecursive  , TT.Plus, TT.Minus),
        new(ParseOperatorType.RightRecursive , TT.Star, TT.Slash),
        new(ParseOperatorType.RightRecursive , TT.Percent),
    };

    public TT? CurrentType => Current.Nullable()?.Type;

    private Parser(LexResult tokens, LangtProject project)
    {
        Project = project;

        Source = tokens;
    }

    public SourceRange Range => Current.Value.Range;
    private List<Token> Prefix {get; set;} = new();

    public int PassLineBreaks(bool prefix)
    {
        // Initialize; clear prefix if necessary
        if(prefix) Prefix.Clear();
        var linecount = 0;

        // Loop while "whitespace" is present
        while(CurrentType is TT.LineBreak or TT.Comment or TT.BlockComment)
        {
            if(CurrentType is TT.LineBreak)
            {
                linecount++;

                // Clear prefix if more than one line break separates comments
                if(prefix && Next.Nullable()?.Type is TT.LineBreak) Prefix.Clear();
            }
            else if(prefix) 
            {
                Prefix.Add(Current!.Value);
            }
            else 
            {
                break;
            }

            Pass();
        }

        return linecount;
    }

    public ASTNode RecoverPoint(Func<ASTNode> pass)
    {
        try {return pass();}
        catch(ParserException exc) 
        {
            Project.Logger.Note($"Found an error at ({Range}), attempting to recover . . . ");
            Project.Diagnostics.Error(exc.Message, Range);
            var r = Range;

            Pass();
            PassAll(c => c.Type is not (TT.LineBreak or TT.EndOfFile or TT.CloseBlock or TT.CloseIndex or TT.CloseParen or TT.Comma));
            PassLineBreaks(false);

            return new ASTInvalid(r);
        }
    }

    public ASTToken Grab() 
    {
        var v = Current.Value;
        Pass();

        var result = new ASTToken(new List<Token>(Prefix), v);
        Prefix.Clear();

        return result;
    }

    public ASTToken Require(
        Predicate<TT?> pred, 
        string? particularError = null, 
        [CallerMemberName] string? passname = "!")
    {
        if(Current.Exists && pred(CurrentType)) return Grab();
        
        var sb = new StringBuilder($"Unexpected {CurrentTypeName} while parsing {passname}");

        if(particularError is not null)
        {
            sb.Append(": ").Append(particularError);
        }

        throw ParserException.Error(sb.ToString());
    }

    public ASTToken Require(TT type, string? particularError = null, [CallerMemberName] string? passname = "!") 
        => Require(t => t == type, particularError, passname);

    public CompilationUnit CompilationUnit(ParserState state)
        => new(StatementGroup(state));

    public Block Block(ParserState state, ParsePass pass)
    {
        PassLineBreaks(false);
        var open = Require(TT.OpenBlock);
        PassLineBreaks(false);

        IEnumerable<ASTNode> Inner() 
        {
            while(Current.Exists && CurrentType is not TT.CloseBlock)
            {
                yield return pass(state);
                PassLineBreaks(false);
            }
        }

        var statements = new List<ASTNode>(Inner());
        var close = Require(TT.CloseBlock);

        return new(open, statements, close);
    }

    public StatementGroup StatementGroup(ParserState state)
    {
        IEnumerable<ASTNode> Inner() 
        {
            while(Current.Exists && CurrentType is not TT.EndOfFile)
            {
                yield return Statement(state);
            }
        }

        return new(new List<ASTNode>(Inner()));
    }

    public ASTNode Statement(ParserState state) => RecoverPoint(() =>
    {
        PassLineBreaks(true);

        ASTNode s;

        if(state.IsProgrammatic)
        {
            s = CurrentType switch
            {
                TT.Let or TT.Extern => Definition(state),
                TT.If => IfStatement(state),
                TT.While => WhileStatement(state),
                TT.Return => Return(state),

                _ => Assignment(state) is var a && (a is Assignment || a is FunctionCall) 
                    ? a 
                    : throw ParserException.Error("Statements must be a variable declaration, control flow statement, function call, or assignment")
            };
        }
        else 
        {
            s = CurrentType switch
            {
                TT.Let or TT.Extern => Definition(state),
                
                TT.Struct => DefineStruct(state),
                TT.Alias => DefineAlias(state),

                TT.Using => UsingDeclaration(state),
                TT.Namespace => NamespaceDeclaration(state),

                _ => throw ParserException.Error($"Unexpected token {CurrentTypeName} found while parsing statement")
            };
        }

        if(PassLineBreaks(false) == 0 && CurrentType is not (TT.EndOfFile or TT.CloseBlock))
        {
            throw ParserException.Error($"Expected terminator for statement" + (CurrentType.HasValue ? $", got {CurrentTypeName}" : ""));
        }
        
        return s;
    });

    public UsingDeclaration UsingDeclaration(ParserState state)
    {
        var usingTok = Require(TT.Using);
        var ns = Namespace(state);

        return new(usingTok, ns);
    }

    public NamespaceDeclaration NamespaceDeclaration(ParserState state)
    {
        var namespaceTok = Require(TT.Namespace);
        var ns = Namespace(state);

        return new(namespaceTok, ns);
    }

    public DefineAlias DefineAlias(ParserState state)
    {
        var aliasTok = Require(TT.Alias);
        var name = Require(TT.Identifier);
        var eq = Require(TT.EqualsSign);
        var type = Type(state);

        return new(aliasTok, name, eq, type);
    }

    public DefineStruct DefineStruct(ParserState state) 
    {
        var structTok = Require(TT.Struct);
        var name = Require(TT.Identifier);
        var gen = GenericParameterSpecification(state);
        PassLineBreaks(false);
        var open = Require(TT.OpenBlock);
        PassLineBreaks(false);
        var fields = SeparatedCollection<DefineStructField>(
            state,
            s => new(Require(TT.Identifier), Type(s)),
            t => t is TT.Comma,
            t => t is TT.CloseBlock
        );
        PassLineBreaks(false);
        var close = Require(TT.CloseBlock);

        return new(structTok, name, gen, open, fields, close);                
    }

    public GenericParameterSpecification? GenericParameterSpecification(ParserState state)
    {
        if(CurrentType is TT.Exclamation)
        {
            var gen   = Grab();
            var open  = Require(TT.LessThan);
            var types = SeparatedCollection(state, p => Require(TT.Identifier), t => t is TT.Comma, t => t is TT.GreaterThan);
            var close = Require(TT.GreaterThan);

            return new(gen, open, types, close);
        }

        return null;
    }

    public Return Return(ParserState state)
    {
        var r = Require(TT.Return);
        ASTNode? expr = null;
        if(CurrentType is not (TT.EndOfFile or TT.LineBreak or TT.CloseBlock)) //TODO: make it clear that this checks for non-expressions!
        {
            expr = Expression(state);
        }

        return new(r, expr);
    }

    public ASTNode Definition(ParserState state)
    {
        var let = Require(t => t is TT.Let or TT.Extern);
        var iden = Require(TT.Identifier);

        if(CurrentType is not TT.OpenParen)
        {
            if(!state.IsProgrammatic) throw ParserException.Error("Cannot define a variable in a definition-level context");

            ASTType? type = null;
            if(CurrentType is not TT.EqualsSign) type = Type(state);
            var eq = Require(TT.EqualsSign);
            var val = Expression(state);

            return new VariableDefinition(let, iden, type, eq, val);
        }
        else
        {
            if(state.IsProgrammatic) throw ParserException.Error("Cannot define a function in a program-level context");

            var open = Require(TT.OpenParen);
            var args = SeparatedCollection(state, ArgumentSpec, t => t is TT.Comma, t => t is TT.CloseParen);

            VarargSpec? vargs = null;
            if(CurrentType is TT.Ellipsis) vargs = new(Grab());

            var close = Require(TT.CloseParen);
            var type = Type(state);

            var newState = state with {IsProgrammatic = true};

            FunctionBody? body = null;

            if(let.Type is not TT.Extern)
            {
                PassLineBreaks(false);
                if(CurrentType is TT.ArrowRight) body = new FunctionExpressionBody(Grab(), Expression(newState));
                else if(CurrentType is TT.OpenBlock) body = new FunctionBlockBody(Block(newState, Statement));
                else throw ParserException.Error("Expected start of function body but got " + CurrentTypeName);
            }

            return new FunctionDefinition(let, iden, open, args, vargs, close, type, body);
        }
    }

    public IfStatement IfStatement(ParserState state)
    {
        var ifTok = Require(TT.If);
        var value = Expression(state);
        PassLineBreaks(false);
        var body = Block(state, Statement);

        ElseStatement? elseStatement = null;

        PassLineBreaks(false);

        if(CurrentType is TT.Else)
        {
            var elseTok = Grab();

            if(CurrentType is TT.If)
            {
                elseStatement = new(elseTok, IfStatement(state));
            }
            else
            {
                elseStatement = new(elseTok, Block(state, Statement));
            }
        }

        return new(ifTok, value, body, elseStatement);
    }

    public WhileStatement WhileStatement(ParserState state)
    {
        var whileTok = Require(TT.While);
        var value = Expression(state);
        PassLineBreaks(false);
        var body = Block(state, Statement);

        return new(whileTok, value, body);
    }

    public ASTType Type(ParserState state)
    {
        ASTType result;

        if(CurrentType is TT.OpenParen)
        {
            Grab();
            result = Type(state);
            Require(TT.CloseParen);
        }
        else if(CurrentType is TT.Star) 
        {
            var star = Grab();

            if(CurrentType is TT.Fn)
            {
                var fn = Grab();
                var open = Require(TT.OpenParen);
                var argspecs = SeparatedCollection(state, Type, t => t is TT.Comma, t => t is TT.CloseParen, false);
                var ellipsis = CurrentType is TT.Ellipsis ? Grab() : null;
                var close = Require(TT.CloseParen);

                var ret = Type(state);

                result = new FunctionPtrType(star, fn, open, argspecs, ellipsis, close, ret);
            }
            else 
            {
                result = new PointerType(star, Type(state));
            }
        }
        else 
        {
            var ns = Namespace(state);
            if(ns is SimpleNamespace simple) 
            {
                result = new SimpleType(simple.Name);
            }
            else if(ns is NestedNamespace nested)
            {
                result = new NestedType(nested.Namespace, nested.Dot, nested.Identifier);
            }
            else 
            { 
                throw new Exception("Unknown namespace type " + ns.GetType().Name);
            }
        }

        if(CurrentType is TT.Pipe)
        {
            var pipe = Grab();
            var next = Type(state);

            result = new OptionType(result, pipe, next);
        }
        else if(CurrentType is TT.Exclamation)
        {
            var gen = Grab();
            var open = Require(TT.LessThan);
            var args = SeparatedCollection(state, Type, t => t is TT.Comma, t => t is TT.GreaterThan, false);
            var close = Require(TT.GreaterThan);

            result = new ConsGenericType(result, gen, open, args, close);
        }

        return result;
    }

    public ASTNamespace Namespace(ParserState state)
    {
        ASTNamespace n = new SimpleNamespace(Require(TT.Identifier));

        while(CurrentType is TT.DoubleColon)
        {
            n = new NestedNamespace(n, Grab(), Require(TT.Identifier));
        }

        return n;
    }

    public ArgumentSpec ArgumentSpec(ParserState state)
    {
        var name = Require(TT.Identifier);
        var type = Type(state);

        return new(name,type);
    }

    public ASTNode Expression(ParserState state) => RecoverPoint(() =>
    {
        ParsePass pass = CastExpression;

        foreach(var spec in Operators.Reverse())
        {
            pass = OperatorPass(spec, pass);
        }

        return pass(state);
    });

    public ParsePass OperatorPass(ParseOperatorSpec spec, ParsePass nextPass)
        => s => Operator(s, spec, nextPass);

    public ASTNode Operator(ParserState state, ParseOperatorSpec spec, ParsePass nextPass)
        => spec.Type switch 
        {
            ParseOperatorType.LeftRecursive => BinaryOperatorLeft(state, spec.TokenTypes, nextPass),
            ParseOperatorType.RightRecursive => BinaryOperatorRight(state, spec.TokenTypes, nextPass),

            ParseOperatorType.UnaryPrefix => UnaryPrefix(state, spec.TokenTypes, nextPass),
            ParseOperatorType.UnaryPostfix => UnaryPostfix(state, spec.TokenTypes, nextPass),

            var t => throw new Exception("Unrecognized operator type " + t)
        };

    public ASTNode BinaryOperatorLeft(ParserState state, TT[] operatorTypes, ParsePass nextPass)
    {
        Stack<(ASTToken? op, ASTNode value)> st = new();
        
        st.Push((null, nextPass(state)));

        while (Current.Exists && operatorTypes.Contains(CurrentType!.Value))
        {
            st.Push((Grab(), nextPass(state)));
        }

        var popped = st.Pop();

        ASTToken? op = popped.op;
        ASTNode val = popped.value;

        while(st.Count > 0)
        {
            popped = st.Pop();

            val = new BinaryOperation(popped.value, op!, val);
            op = popped.op;
        }

        return val;
    }
    public ASTNode BinaryOperatorRight(ParserState state, TT[] operatorTypes, ParsePass nextPass)
    {
        var lhs = nextPass(state);
        if(Current.Exists && operatorTypes.Contains(CurrentType!.Value))
        {
            return new BinaryOperation(lhs, Grab(), BinaryOperatorRight(state, operatorTypes, nextPass));
        }

        return lhs;
    }
    public ASTNode UnaryPrefix(ParserState state, TT[] operatorTypes, ParsePass nextPass)
    {
        if(Current.Exists && operatorTypes.Contains(CurrentType!.Value))
        {
            return new UnaryOperation(Grab(), UnaryPrefix(state, operatorTypes, nextPass));
        }

        return nextPass(state);
    }
    public ASTNode UnaryPostfix(ParserState state, TT[] operatorTypes, ParsePass nextPass)
    {
        var v = nextPass(state);

        while(Current.Exists && operatorTypes.Contains(CurrentType!.Value))
        {
            v = new UnaryOperation(Grab(), v);
        }

        return v;
    }

    public ASTNode CastExpression(ParserState state) 
    {
        var ret = Functionlike(state);

        while(CurrentType is TT.As)
        {
            var asTok = Grab();
            var type = Type(state);

            ret = new CastExpression(ret, asTok, type);
        }

        return ret;
    }
    
    public ASTNode Assignment(ParserState state)
    {
        var v = Expression(state);

        if(CurrentType is TT.EqualsSign)
        {
            var eq = Grab();
            var expr = Expression(state);

            return new Assignment(v, eq, expr);
        }

        return v;
    }

    public ASTNode Functionlike(ParserState state)
    {
        var ret = PrimaryExpression(state);

        while(CurrentType is TT.OpenParen or TT.OpenIndex or TT.Dot or TT.DoubleColon)
        {
            if(CurrentType is TT.OpenParen)
            {
                var open = Grab();
                var args = SeparatedCollection(state with {AllowCommaExpressions = false}, Expression, t => t is TT.Comma, t => t is TT.CloseParen);
                var close = Require(TT.CloseParen);

                ret = new FunctionCall(ret, open, args, close);
            }
            else if(CurrentType is TT.OpenIndex)
            {
                var open = Grab();
                var index = SeparatedCollection(state with {AllowCommaExpressions = false}, Expression, t => t is TT.Comma, t => t is TT.CloseIndex);
                var close = Require(TT.CloseIndex);

                ret = new IndexExpression(ret, open, index, close);
            }
            else if(CurrentType is TT.Dot)
            {
                var dot = Grab();
                var iden = Require(TT.Identifier);

                ret = new DotAccess(ret, dot, iden);
            }
            else if(CurrentType is TT.DoubleColon)
            {
                var dot = Grab();
                var iden = Require(TT.Identifier);

                ret = new StaticAccess(ret, dot, iden);
            }
        }

        return ret;
    }

    // TODO: determine brackus-naur notation for language

    public ASTNode PrimaryExpression(ParserState state) => RecoverPoint(() => CurrentType switch
    {
        TT.OpenParen => ParentheticExpression(state),
        TT.Sizeof => Sizeof(state),

        TT.Identifier when Next.Nullable()?.Type is TT.OpenBlock => StructCreate(state), 
            //TODO: this does not account for types in namespaces!
            //TODO: refactor to allow for struct creation using static references.
            //TODO: do so by having common types (static '::' access) have corresponding 'wrapper' types
        TT.Identifier => new Identifier(Grab()),
        TT.Integer or TT.Decimal
            => new NumericLiteral(Grab()),
        TT.String => new StringLiteral(Grab()),

        TT.True or TT.False
            => new BooleanLiteral(Grab()),

        _ => throw ParserException.Error($"Unexpected {CurrentTypeName} found while parsing primary expression")
    });

    public Sizeof Sizeof(ParserState state) 
    {
        var s = Require(TT.Sizeof);
        var t = Type(state);

        return new(s, t);
    }

    public StructInitializer StructCreate(ParserState state)
    {
        var type = Type(state);
        var open = Require(TT.OpenBlock);
        var args = SeparatedCollection(state, Expression, t => t is TT.Comma, t => t is TT.CloseBlock);
        var close = Require(TT.CloseBlock, "expected matching '}' for earlier '{'");

        return new(type, open, args, close);
    }

    public ParentheticExpression ParentheticExpression(ParserState state)
    {
        var open = Grab();
        var expr = Expression(state);
        var close = Require(TT.CloseParen, "expected matching ')' for earlier '('");

        return new ParentheticExpression(open, expr, close);
    }

    public SeparatedCollection<T> SeparatedCollection<T>(ParserState state, ParsePass<T> pass, Predicate<TT> seperatorPred, Predicate<TT> endPred, bool lineBreaksAfterSep = true)
        where T : ASTNode
    {
        List<ASTNode> b = new();

        if(Current.Exists && !endPred(CurrentType!.Value))
        {
            b.Add(pass(state)!);
            while(Current.Exists && seperatorPred(CurrentType.Value))
            {
                b.Add(Grab());
                
                if(lineBreaksAfterSep) PassLineBreaks(true);

                b.Add(pass(state)!);
            }
        }

        return new(b);
    }


    public static CompilationUnit Parse(LexResult tokens, LangtProject project)
    {
        var p = GetParser(tokens, project);
        CompilationUnit res;

        try
        {
            res = p.CompilationUnit(ParserState.Start);
        } 
        catch(Exception e) 
        {
            throw new Exception(
                "An exception occured while parsing @ " + p.Current.Value + ":",
                e
            );
        }

        return res;
    }
    
    public static Parser GetParser(LexResult tokens, LangtProject project)
        => new(tokens, project);
}
