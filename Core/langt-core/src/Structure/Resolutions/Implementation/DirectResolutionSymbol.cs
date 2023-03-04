namespace Langt.Structure;

public class DirectResolutionSymbol : Symbol<IFullNamed>
{
    public DirectResolutionSymbol(SourceRange range, IScope scope, string name) 
    {
        Range = range;
        Name = name;
        HoldingScope = scope;
    }

    public string Name {get; init;}
    public IScope HoldingScope {get; init;}

    public override Result<IFullNamed> Unravel(Context ctx) 
    {
        return HoldingScope.Resolve(Name, Range, ctx);
    }
}


