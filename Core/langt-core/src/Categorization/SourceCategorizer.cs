namespace Langt.Lexing;

public class SourceCategorizer
{
    public List<SourceCategorization> Categorizations {get; init;} = new();

    public void Categorize(SourceCategorization categorization)
        => Categorizations.Add(categorization);
    public void Categorize(SourceRange range, TokenCategory category)
        => Categorize(new(range, category));
}