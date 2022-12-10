using System.Text.RegularExpressions;
using Langt.Structure;
using Langt.Structure.Visitors;

using TT = Langt.Lexing.TokenType;

namespace Langt.Lexing;

public interface ITokenlike // TODO: IMPLEMENT
{

}

public readonly record struct Token(TT Type, SourceRange Range) : ISourceRanged, IElement<VisitDumper>
{
    public override string ToString()
        => $"Token({Type}, {Range}): \"{Range.Content.ToString().ReplaceLineEndings("(newline)")}\"";

    public void OnVisit(VisitDumper dumper)
    {
        dumper.PutString("\"" + ContentStr + "\"");
    }

    public ReadOnlySpan<char> Content => Range.Content;
    public string ContentStr => Range.Content.ToString();

    public TokenCategory Category => Type switch 
    {
        TT.And or TT.Or or TT.Not or TT.As or TT.Ptrto or TT.Let or TT.Extern or TT.If or TT.Else or TT.While or TT.Return
            => TokenCategory.Keyword,
        TT.OpenBlock or TT.OpenIndex or TT.OpenParen or TT.CloseBlock or TT.CloseIndex or TT.CloseParen
            => TokenCategory.Brace,
        TT.Plus or TT.Minus or TT.Star or TT.Slash or TT.Percent or TT.DoubleEquals or TT.GreaterEqual or TT.GreaterThan or TT.LessEqual or TT.LessThan
            => TokenCategory.Operator,
        TT.Identifier
            => TokenCategory.Identifier,
        TT.String or TT.Char
            => TokenCategory.String,
        TT.Integer or TT.Decimal
            => TokenCategory.NumericLiteral,

        _ => TokenCategory.Other
    };
}

