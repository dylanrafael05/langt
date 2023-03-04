namespace Langt.Structure;

public interface ISymbol : ISourceRanged
{
    /// <summary>
    /// The symbol this instance is "based" off of.
    /// </summary>
    /// <remarks>
    /// The reason for this properties' existence is to allow for
    /// the "decoration" of a symbol (by creating a wrapping symbol)
    /// which adds extra validation logic to the symbol's <see cref="Unravel(Context)"/>, 
    /// like <see cref="SymbolExtensions.As{T}(ISymbol)"/>, while simultaneously
    /// allowing inspection of the information of the symbol instance.
    /// </remarks>
    ISymbol BaseSymbol {get;}
    Result<object> Unravel(Context ctx);
}

public interface ISymbol<T> : ISymbol
{
    Result<object> ISymbol.Unravel(Context ctx)
        => Unravel(ctx).As<object>();
    new Result<T> Unravel(Context ctx);
}