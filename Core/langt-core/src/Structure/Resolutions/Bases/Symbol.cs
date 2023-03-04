namespace Langt.Structure;

public abstract class Symbol<T> : ISymbol<T>
{
    public virtual SourceRange Range { get; init; }
    public abstract Result<T> Unravel(Context ctx);

    public virtual ISymbol BaseSymbol => this;
}


