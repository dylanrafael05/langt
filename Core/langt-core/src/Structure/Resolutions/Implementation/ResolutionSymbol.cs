namespace Langt.Structure;

public class ResolutionSymbol : Symbol<IFullNamed>
{
    public ResolutionSymbol(SourceRange range, ISymbol<IScope> scope, string name) 
    {
        Range = range;
        Name = name;
        SearchScope = scope;
    }

    public string Name {get; init;}
    public ISymbol<IScope> SearchScope {get; init;}

    public override Result<IFullNamed> Unravel(Context ctx) 
    {
        var sres = SearchScope.Unravel(ctx);
        if(!sres) return sres.As<IFullNamed>();

        return new DirectResolutionSymbol(Range, sres.Value, Name).Unravel(ctx);
    }
}


