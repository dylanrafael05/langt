namespace Langt.Structure;

public abstract class ImmediateResolvable : Resolvable
{
    public ImmediateResolvable()
    {
        Completion = CompletionState.Complete;
    }

    public sealed override Result Complete(Context ctx)
        => Result.Success();
}


