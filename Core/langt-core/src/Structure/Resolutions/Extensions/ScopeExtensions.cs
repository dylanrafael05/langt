namespace Langt.Structure;

public static class ScopeExtensions
{
    public static ISymbol<IFullNamed> ResolveSymbol<T>(this ISymbol<T> sc, string name, SourceRange range) where T : IScope
        => new ResolutionSymbol(range, sc.As<IScope>("scope"), name);
    public static ISymbol<IFullNamed> ResolveSymbol(this IScope sc, string name, SourceRange range)
        => new DirectResolutionSymbol(range, sc, name);

    public static Result DefineProxy(this IScope sc, IFullNamed item, SourceRange range)
        => sc.Define(new ProxyResolution(sc, item), range);
}


