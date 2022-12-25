namespace Langt;

public record struct Diagnostic(MessageSeverity Severity, string Message, SourceRange Range) : IResultMetadata, IResultError
{
    public static Diagnostic Error(string message, SourceRange range) => new(MessageSeverity.Error, message, range);
    public static Diagnostic Warning(string message, SourceRange range) => new(MessageSeverity.Warning, message, range);
    public static Diagnostic Note(string message, SourceRange range) => new(MessageSeverity.Note, message, range);

    public IResultMetadata? TryDemote()
        => this;
}
