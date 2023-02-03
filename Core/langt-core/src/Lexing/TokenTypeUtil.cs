using TT = Langt.Lexing.TokenType;

namespace Langt.Lexing;

public static class TokenTypeUtil
{
    public static string GetReadableName(this TT t) => t switch 
    {
        TT.Ampersand => "'&'",

        TT.OpenParen => "'('",
        TT.OpenIndex => "'['",
        TT.OpenBlock => "'{'",
        
        TT.CloseParen => "')'",
        TT.CloseIndex => "']'",
        TT.CloseBlock => "'}'",
        
        TT.String     => "string",
        TT.Identifier => "identifier",
        
        TT.Decimal => "decimal number",
        TT.Integer => "integer number",

        TT.Plus    => "'+'",
        TT.Minus   => "'-'",
        TT.Star    => "'*'",
        TT.Slash   => "'/'",
        TT.Percent => "'%'",

        TT.Colon => "':'",

        TT.Dot   => "'.'",
        TT.Comma => "','",

        TT.EqualsSign   => "'='",
        TT.DoubleEquals => "'=='",
        TT.NotEquals    => "'!='",
        
        TT.GreaterThan  => "'>'",
        TT.GreaterEqual => "'>='",
        
        TT.LessThan  => "'<'",
        TT.LessEqual => "'<='",

        TT.ArrowRight => "'=>'",
        TT.Ellipsis  => "'...'",

        _ => "'" + t.ToString().ToLower() + "'"
    };

    public static string GetReadableName(this Token t) => t.Type switch 
    {
        TT.Identifier or TT.String or TT.Integer or TT.Decimal => t.ContentStr,
        _ => t.Type.GetReadableName()
    };
}