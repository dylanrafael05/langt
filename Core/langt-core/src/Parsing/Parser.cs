using System;
using Langt.AST;
using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure;
using System.Runtime.CompilerServices;
using TT = Langt.Lexing.TokenType;

namespace Langt.Parsing;

public delegate ASTNode ParsePass(ParserState state);
public delegate T ParsePass<T>(ParserState state) where T : ASTNode;

// TODO: handle (trivia) properly!
public sealed class Parser : LookaheadListStream<Token>, IProjectDependency
{
    public LangtProject Project {get; init;}
    public string CurrentTypeName => CurrentType?.GetReadableName() ?? "null token";

    public static readonly OperatorSpec[] Operators = new OperatorSpec[] 
    {
        new(OperatorType.UnaryPrefix    , TT.Not),
        new(OperatorType.LeftRecursive  , TT.Or),
        new(OperatorType.LeftRecursive  , TT.And),
        new(OperatorType.LeftRecursive  , TT.DoubleEquals, TT.GreaterThan, TT.GreaterEqual, TT.LessEqual, TT.LessThan, TT.NotEquals),
        new(OperatorType.LeftRecursive  , TT.Plus, TT.Minus),
        new(OperatorType.RightRecursive , TT.Star, TT.Slash),
        new(OperatorType.RightRecursive , TT.Percent),
    };
    public TT? CurrentType
        => Current.Nullable()?.Type;

    private Parser(LexResult tokens, LangtProject project)
    {
        Project = project;

        Source = tokens;
    }

    public SourceRange Range => Current.Value.Range;

    public int PassLineBreaks() => PassAll(t => t.Type is TT.LineBreak);
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
            PassLineBreaks();

            return new ASTInvalid(r);
        }
    }

    public ASTToken Grab() 
    {
        var v = Current.Value;
        index++;
        return new(v);
    }

    public ASTToken Require(
        Predicate<TT?> pred, 
        string? particularError = null, 
        [CallerMemberName] string? passname = "!") // TODO: improve errors generated here with a 'context' string
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
        PassLineBreaks();

        var open = Require(TT.OpenBlock);

        IEnumerable<ASTNode> Inner() 
        {
            while(Current.Exists && CurrentType is not TT.CloseBlock)
            {
                yield return pass(state);
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

    // TODO: reimplement dumping via a visitor

    public ASTNode Statement(ParserState state) => RecoverPoint(() =>
    {
        PassLineBreaks();

        ASTNode s;

        if(state.IsProgrammatic)
        {
            s = CurrentType switch
            {
                TT.Let or TT.Extern => Definition(state),
                TT.If => IfStatement(state),
                TT.While => WhileStatement(state),
                TT.Return => Return(state),

                // TODO; this might mess up internal state of parser
                _ => Assignment(state)
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

        if(PassLineBreaks() == 0 && CurrentType is not (TT.EndOfFile or TT.CloseBlock))
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
        PassLineBreaks();
        var open = Require(TT.OpenBlock);
        PassLineBreaks();
        var fields = SeparatedCollection<DefineStructField>(
            state,
            s => new(Require(TT.Identifier), Type(s)),
            t => t is TT.Comma,
            t => t is TT.CloseBlock
        );
        PassLineBreaks();
        var close = Require(TT.CloseBlock);

        return new(structTok, name, open, fields, close);                
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

            return new DefineVariable(let, iden, type, eq, val);
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
                PassLineBreaks();
                if(CurrentType is TT.EqualsSign) body = new FunctionExpressionBody(Grab(), Expression(newState));
                else if(CurrentType is TT.OpenBlock) body = new FunctionBlockBody(Block(newState, Statement));
                else throw ParserException.Error("Expected start of function body but got " + CurrentTypeName);
            }

            return new DefineFunction(let, iden, open, args, vargs, close, type, body);
        }
    }

    public IfStatement IfStatement(ParserState state)
    {
        var ifTok = Require(TT.If);
        var value = Expression(state);
        PassLineBreaks();
        var body = Block(state, Statement);

        ElseStatement? elseStatement = null;

        PassLineBreaks();

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
        PassLineBreaks();
        var body = Block(state, Statement);

        return new(whileTok, value, body);
    }

    public ASTType Type(ParserState state)
    {
        if(CurrentType is TT.Star) 
        {
            return new PointerType(Grab(), Type(state));
        }
        else if(CurrentType is TT.OpenParen)
        {
            var open = Grab();
            var argspecs = SeparatedCollection(state, ArgumentSpec, t => t is TT.Comma, t => t is TT.CloseParen, false);
            var ellipsis = CurrentType is TT.Ellipsis ? Grab() : null;
            var close = Require(TT.CloseParen);

            var ret = Type(state);

            return new FunctionPtrType(open, argspecs, ellipsis, close, ret);
        }

        var ns = Namespace(state);
        if(ns is SimpleNamespace simple) 
        {
            return new SimpleType(simple.Name);
        }
        else if(ns is NestedNamespace nested)
        {
            return new DotType(nested.Namespace, nested.Dot, nested.Identifier);
        }
        
        throw new Exception("Unknown namespace type " + ns.GetType().Name);
    }

    public ASTNamespace Namespace(ParserState state)
    {
        ASTNamespace n = new SimpleNamespace(Require(TT.Identifier));

        while(CurrentType is TT.Dot)
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

    // TODO: change this to return only direct values
    public ASTNode Expression(ParserState state) => RecoverPoint(() =>
    {
        ParsePass pass = CastExpression;

        foreach(var spec in Operators.Reverse())
        {
            pass = OperatorPass(spec, pass);
        }

        return pass(state);
    });

    public ParsePass OperatorPass(OperatorSpec spec, ParsePass nextPass)
        => s => Operator(s, spec, nextPass);

    public ASTNode Operator(ParserState state, OperatorSpec spec, ParsePass nextPass)
        => spec.Type switch 
        {
            OperatorType.LeftRecursive => BinaryOperatorLeft(state, spec.TokenTypes, nextPass),
            OperatorType.RightRecursive => BinaryOperatorRight(state, spec.TokenTypes, nextPass),

            OperatorType.UnaryPrefix => UnaryPrefix(state, spec.TokenTypes, nextPass),
            OperatorType.UnaryPostfix => UnaryPostfix(state, spec.TokenTypes, nextPass),

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

        while(CurrentType is TT.OpenParen or TT.OpenIndex or TT.Dot)
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
                var index = Expression(state);
                var close = Require(TT.CloseIndex);

                ret = new IndexExpression(ret, open, index, close);
            }
            else if(CurrentType is TT.Dot)
            {
                var dot = Grab();
                var iden = Require(TT.Identifier);

                ret = new DotAccess(ret, dot, iden);
            }
        }

        return ret;
    }

    // TODO: determine brackus-naur notation for language

    public ASTNode PrimaryExpression(ParserState state) => RecoverPoint(() => CurrentType switch
    {
        TT.OpenParen => ParentheticExpression(state),
        TT.Ampersand => PtrTo(state),

        TT.Identifier when Next.Nullable()?.Type is TT.OpenBlock => StructCreate(state), //TODO: see lower todo
        TT.Identifier => new Identifier(Grab()),
        TT.Integer or TT.Decimal
            => new NumericLiteral(Grab()),
        TT.String => new StringLiteral(Grab()),

        TT.True or TT.False
            => new BooleanLiteral(Grab()),

        _ => throw ParserException.Error($"Unexpected {CurrentTypeName} found while parsing primary expression")
    });

    public StructInitializer StructCreate(ParserState state)
    {
        var type = Type(state); // TODO: ENSURE THIS IS A TYPE BEFORE PARSING!
        var open = Require(TT.OpenBlock);
        var args = SeparatedCollection(state, Expression, t => t is TT.Comma, t => t is TT.CloseBlock);
        var close = Require(TT.CloseBlock);

        return new(type, open, args, close);
    }

    public ParentheticExpression ParentheticExpression(ParserState state)
    {
        var open = Grab();
        var expr = Expression(state);
        var close = Require(TT.CloseParen);

        return new ParentheticExpression(open, expr, close);
    }

    public PtrTo PtrTo(ParserState state)
        => new(Require(TT.Ampersand), Require(TT.Identifier));

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
                
                if(lineBreaksAfterSep) PassLineBreaks();

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
