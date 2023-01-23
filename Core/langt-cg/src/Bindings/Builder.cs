namespace Langt.CG.Bindings;

public abstract class Builder<TIn, TOut>
{
    public required CodeGenerator CG {get; init;}
    public abstract TOut Build(TIn fn);
}
