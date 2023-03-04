namespace Langt.Structure;

public interface ISymbolProvider<T>
{
    public ISymbol<T> GetSymbol(Context ctx);
}


