using Langt.Message;

namespace Langt.Structure;

public class SimpleScope : IScope
{
    private readonly Dictionary<string, IResolvable> items = new();

    public required IScope? Parent {get; init;}

    public IEnumerable<IResolvable> Items(Context ctx)
    {
        foreach(var name in items.Keys)
        {
            yield return ResolveDirect(name, SourceRange.Default, ctx, false).Expect();
        }
    }
    
    public Result<IResolvable> ResolveDirect(string identifier, SourceRange range, Context ctx, bool upwards = true)
    {
        if (items.TryGetValue(identifier, out var resultant))
        {
            if(resultant.Completion == CompletionState.Incomplete)
            {
                var res = resultant.Complete(ctx);

                if(!res) return Result.Blank<IResolvable>().WithDataFrom(res);
            }

            return Result.Success(resultant);
        }
        else if (upwards && Parent is not null)
        {
            return Parent.ResolveDirect(identifier, range, ctx, true);
        }
        else 
        {
            return Result.Error<IResolvable>(Diagnostic.Error(Messages.Get("no-found", identifier), range));
        }
    }
    public Result<IFullNamed> Resolve(string identifier, SourceRange range, Context ctx, bool upwards = true)
        => ResolveDirect(identifier, range, ctx, upwards).Map(k => k.UnwrapProxy());

    public Result Define(IResolvable value, SourceRange range)
    {
        if (items.ContainsKey(value.Name))
        {
            return Result.Error(Diagnostic.Error(Messages.Get("redefine", value.Name), range));
        }

        items.Add(value.Name, value);

        return Result.Success();
    }
}


