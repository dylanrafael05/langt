namespace Langt;

public record struct Diagnostic(DiagnosticSeverity Severity, string Message, SourceRange Range)
{
    /// <summary>
    /// Get a unique (or as unique as possible) key for sorting this item.
    /// </summary>
    /// <returns></returns>
    public double SortingKey => (Range.Source.Name.GetHashCode() % 1000) + ((Range.CharStart + 1) / (double)(Range.Source.Content.Length + 3));
}
