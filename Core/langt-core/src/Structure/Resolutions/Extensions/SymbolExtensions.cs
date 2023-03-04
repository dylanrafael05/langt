namespace Langt.Structure;

public static class SymbolExtensions
{
    private readonly struct CastSymbol<T> : ISymbol<T>
    {
        public ISymbol Source {get; init;}
        public string TypeName {get; init;}

        public SourceRange Range => Source.Range;

        public ISymbol BaseSymbol => Source.BaseSymbol;

        public Result<T> Unravel(Context ctx)
        {
            var k = Source.Unravel(ctx);
            if(!k) return k.ErrorCast<T>();

            if(k.Value is not T)
            {
                return k
                    .WithError(Diagnostic.Error($"Expected to find a {TypeName} but did not", Range))
                    .ErrorCast<T>();
            }

            return k.As<T>();
        }
    }

    public static ISymbol<T> As<T>(this ISymbol symbol, string typename)
    {
        return new CastSymbol<T>
        {
            Source = symbol,
            TypeName = typename
        };
    }

    public static ISymbol<T> As<T>(this ISymbol symbol) where T : IResolvable
        => symbol.As<T>(T.TypeName);
}


