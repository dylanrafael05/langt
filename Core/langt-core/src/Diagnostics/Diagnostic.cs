using Langt.Lexing;

namespace Langt;

public record struct Diagnostic(MessageSeverity Severity, string Message, SourceRange Range) : IResultMetadata, IResultError
{
    public static Diagnostic Error(string message, SourceRange range) => new(MessageSeverity.Error, message, range);
    public static Diagnostic Warning(string message, SourceRange range) => new(MessageSeverity.Warning, message, range);
    public static Diagnostic Note(string message, SourceRange range) => new(MessageSeverity.Note, message, range);

    public IResultMetadata? TryDemote()
        => this;

    public string GetDisplayText(LangtProject proj)
    {
        // TODO; use binary search; generally improve this algorithm
        const int CharPaddingBehind = 40;
        const int CharPaddingFront  = 20;

        var range = Range;

        var file = proj.Files.First(f => f.Source == range.Source);

        var (si, st) = file.Lex.Indexed().First(t => t.Value.Range.CharStart > range.CharStart - CharPaddingBehind && t.Value.Range.LineStart == range.LineStart);
        var (ei, et) = file.Lex.Indexed().First(t => t.Value.Range.CharStart > range.CharStart + CharPaddingFront  || t.Value.Range.LineEnd > range.LineStart || t.Value.Type is TokenType.EndOfFile);

        var text = file.Source.Content
            .PaddedSubstring(st.Range.CharStart, et.Range.CharEnd)
            .Replace('\n', ' ')
            .Trim('\n', '\r');
        
        var underline = new string(
            text.Indexed()
                .Select(k => k.Index + st.Range.CharStart is var i && range.CharStart <= i && i < range.CharEnd ? '^' : '-')
                .ToArray()
        );

        var ellipsis = si != 0 && file.Lex[si-1].Range.LineStart == st.Range.LineStart ? " . . . " : "";

        return new StringBuilder()
            .AppendLine(Message)
            .AppendLine("     at " + Range.Source.Name + ":" + Range.LineStart + ":" + (Range.ColumnStart+1))
            .AppendLine()
            .AppendLine($" {Range.LineStart,5} | " + ellipsis + text)
            .AppendLine($"       | " + ellipsis + underline)
            .ToString();
    }
}
