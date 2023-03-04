namespace Langt.Structure;

public interface ISymbol 
{
    Result<object> Unravel(Context ctx);
}

public interface ISymbol<T> : ISymbol
{
    Result<object> ISymbol.Unravel(Context ctx)
        => Unravel(ctx).As<object>();
    new Result<T> Unravel(Context ctx);
}