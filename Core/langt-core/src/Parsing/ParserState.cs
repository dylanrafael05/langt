namespace Langt.Parsing;

public record ParserState(bool IsProgrammatic, bool AllowCommaExpressions)
{
    public static ParserState Start => new(false, true);
}
