namespace Langt;

/// <summary>
/// Represents some object which has an associated source range.
/// </summary>
public interface ISourceRanged 
{
    SourceRange Range {get;}
}
