namespace Langt.Structure;

public interface IResolvable : IResolutionlike
{
    CompletionState Completion {get;}
    Result Complete(Context ctx);

    static virtual string TypeName => "item";
}


