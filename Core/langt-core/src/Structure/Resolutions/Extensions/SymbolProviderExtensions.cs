namespace Langt.Structure;

public static class SymbolProviderExtensions
{
    public static Result<T> UnravelSymbol<T>(this ISymbolProvider<T> self, Context ctx)
        => self.GetSymbol(ctx).Unravel(ctx);
}


