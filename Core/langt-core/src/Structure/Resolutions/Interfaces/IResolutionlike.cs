namespace Langt.Structure;

public interface IResolutionlike : IFullNamed
{
    IScope HoldingScope {get;}
    string? Documentation {get;}
    SourceRange? DefinitionRange {get;}
}


