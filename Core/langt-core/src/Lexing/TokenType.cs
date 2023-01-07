namespace Langt.Lexing;

public enum TokenType
{
    Error,
    
    Ptrto,

    Const,
    Type,
    Struct,

    ArrowRight,

    Let,
    Extern,

    Return,

    EqualsSign,
    DoubleEquals,
    NotEquals,

    Sizeof,

    Plus,
    Minus,
    Star,
    Slash,
    Percent,

    GreaterThan,
    GreaterEqual,
    LessThan,
    LessEqual,

    And,
    Or,
    Not,

    Colon,

    OpenBlock,
    CloseBlock,

    OpenIndex,
    CloseIndex,

    Identifier,

    Integer,
    Decimal,
    True,
    False,

    If,
    Else,
    While,
    For,
    Select,

    String,
    Char,
    AsciiChar,

    Alias,

    As,
    
    Ampersand,

    Namespace,
    Using,

    LineBreak,
    Comment,
    BlockComment,

    OpenParen,
    CloseParen,

    Dot,
    Comma,
    Ellipsis,

    EndOfFile,
}
