using System.Diagnostics;
using Langt.Codegen;
using Langt.Structure;
using TT = Langt.Lexing.TokenType;

namespace Langt.Lexing;

public sealed class Lexer : LookaheadListStream<char>, IProjectDependency
{
    public LangtProject Project {get; init;}

    // DATA //
    private readonly Source src;

    private int lineIndex = 1;
    private int columnIndex = 0;

    public override void Pass(int count = 1)
    {
        base.Pass(count);
        columnIndex += count;
    }

    private SourcePosition startPos;
    private SourcePosition CurrentPos => new(index, lineIndex, columnIndex, src);
    private SourceRange Range => SourceRange.From(startPos, CurrentPos);

    // CONSTRUCTOR //
    private Lexer(Source src, LangtProject project)
    {
        Project = project;

        Source = src.Content.ToCharArray();
        this.src = src;
        startPos = new(0, 1, 0, src);
    }

    // FUNCTIONALITY //
    private void BeginToken()
    {
        startPos = CurrentPos;
    }

    // Tokenizers
    private Token Error(string message) 
    {
        Project.Diagnostics.Error(message, Range);
        return Grab(1, TT.Error);
    }

    private Token BuildToken(TT type)
        => new(type, Range);
    private Token Grab(int count, TT type)
    {
        index += count;
        return BuildToken(type);
    }
    private Token GrabLineBreak(int count)
    {
        index += count;
        lineIndex++;
        columnIndex = 0;
        return BuildToken(TT.LineBreak);
    }
    private Token GrabAll(Predicate<char> pred, TT type)
    {
        PassAll(pred);
        return BuildToken(type);
    }
    private Token GrabAll(Predicate<char> pred, Func<string, TT> typeMatcher)
    {
        PassAll(pred);
        return BuildToken(typeMatcher(Range.Content.ToString()));
    }
    private Token GrabWhile(Func<bool> pred, TT type)
    {
        PassWhile(pred);
        return BuildToken(type);
    }
    private Token GrabAfter(Action func, TT type)
    {
        func();
        return BuildToken(type);
    }
    private Token GrabAfter(Func<TT> func)
    {
        return BuildToken(func());
    }

    private Token? GrabNone(Action func) 
    {
        func();
        return null;
    }

    // Sublexers
    private (Token? token, bool isDone) LexWhitespace() => (Current.Nullable(), Next.Nullable()) switch
    {
        ('\r', '\n') or ('\n', '\r') => (GrabLineBreak(2), false),
        ('\n', _   ) or ('\r', _   ) => (GrabLineBreak(1), false),
        (' ' , _   ) or ('\t', _   ) => (GrabNone(() => PassAll(ch => ch is ' ' or '\t')), false),
        ('#' , _   )                 => (GrabNone(() => PassAll(ch => ch is not '\r' and not '\n')), false),
        _                            => (null, true)
    };

    private Token LexNonwhitespace() => Current.Nullable() switch 
    {
        '=' => Next.Nullable() switch
        {
            '=' => Grab(2, TT.DoubleEquals),
            _   => Grab(1, TT.EqualsSign)
        },

        '.' => (Next.Nullable(), Get(2).Nullable()) switch
        {
            ('.', '.') => Grab(3, TT.Ellipsis),
            _          => Grab(1, TT.Dot),
        },

        '!' when Next.Nullable() is '=' => Grab(2, TT.NotEquals),

        '>' => Next.Nullable() switch 
        {
            '=' => Grab(2, TT.GreaterEqual),
            _   => Grab(1, TT.GreaterThan)
        },
        '<' => Next.Nullable() switch 
        {
            '=' => Grab(2, TT.LessEqual),
            _   => Grab(1, TT.LessThan)
        },

        ',' => Grab(1, TT.Comma),
        
        '+' => Grab(1, TT.Plus),
        '-' => Grab(1, TT.Minus),
        '*' => Grab(1, TT.Star),
        '/' => Grab(1, TT.Slash),
        '%' => Grab(1, TT.Percent),

        '(' => Grab(1, TT.OpenParen),
        ')' => Grab(1, TT.CloseParen),

        '[' => Grab(1, TT.OpenIndex),
        ']' => Grab(1, TT.CloseIndex),

        '{' => Grab(1, TT.OpenBlock),
        '}' => Grab(1, TT.CloseBlock),

        ':' => Grab(1, TT.Colon),

        '&' => Grab(1, TT.Ampersand),

        '\'' => Next.Nullable() switch
        {
            '\\' => Grab(4, TT.Char),
            _    => Grab(3, TT.Char)
        },

        '"' => GrabAfter(() => {

            do
            {
                index++;

                PassAll(ch => ch is not '"' and not '\n' and not '\r');

                if(Current.Nullable() is '\n' or '\r')
                {
                    Project.Diagnostics.Error("Unterminated string literal.", Range);
                }
            } 
            while(Last.Nullable() is '\\' && Current.Nullable() is '"');

            index++;

        }, TT.String),

        (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '_' or '$'                    
                            => GrabAll(ch => char.IsLetterOrDigit(ch) || ch is '_' or '$', MapKeywordType),
        
        >= '0' and <= '9'   => GrabAfter(() => 
        {
            PassAll(ch => ch is >= '0' and <= '9');

            if(Current is (true, '.'))
            {
                index++;
                PassAll(ch => ch is >= '0' and <= '9');

                return TT.Decimal;
            }

            return TT.Integer;
        }),

        null => Grab(1, TT.EndOfFile),

        _ => Error($"Unknown character '{Current.Value}'")
    };

    private TT MapKeywordType(string str) => str switch 
    {
        "let"    => TT.Let,
        "const"  => TT.Const,
        "extern" => TT.Extern,

        "ptrto"  => TT.Ptrto,

        "alias"  => TT.Alias,
        "struct" => TT.Struct,
        "type"   => TT.Type,

        "sizeof" => TT.Sizeof,

        "and"    => TT.And,
        "not"    => TT.Not,
        "or"     => TT.Or,

        "if"     => TT.If,
        "else"   => TT.Else,

        "while"  => TT.While,
        "for"    => TT.For,

        "select" => TT.Select,

        "as"     => TT.As,

        "return" => TT.Return,

        "true"   => TT.True,
        "false"  => TT.False,

        "namespace" => TT.Namespace,
        "using"     => TT.Using,

        _ => TT.Identifier
    };

    // Full Lexer
    private IEnumerable<Token> LexInternal()
    {
        TT last = TT.Error;
        while(last != TT.EndOfFile)
        {
            BeginToken();

            while(LexWhitespace() is var r && !r.isDone) 
            {
                if(r.token is Token ws) yield return ws;
            }

            BeginToken();

            var tok = LexNonwhitespace();
            yield return tok;

            last = tok.Type;
        }
    }

    // EXPOSED FUNCTIONALITY //
    public static LexResult Lex(Source src, LangtProject project)
    {
        var lexer = GetLexer(src, project);
        var result = lexer.LexInternal();
        return new(result);
    }

    public static Lexer GetLexer(Source src, LangtProject project) 
        => new(src, project);
}
