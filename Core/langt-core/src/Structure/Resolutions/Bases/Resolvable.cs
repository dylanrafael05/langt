namespace Langt.Structure;

public abstract class Resolvable : IResolvable
{
    public virtual CompletionState Completion {get; protected set;}

    public required string Name {get; init;}
    public required IScope HoldingScope {get; init;}

    public string? Documentation {get; init;}
    public SourceRange? DefinitionRange {get; init;}

    public virtual string DisplayName => Name;
    public virtual string FullName => this.FullNameSimple();

    Result IResolvable.Complete(Context ctx)
    {
        Completion = CompletionState.InProgress;
        var r = Complete(ctx);
        Completion = CompletionState.Complete;

        return r;
    }

    public abstract Result Complete(Context ctx);
}


