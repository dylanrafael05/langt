namespace Langt.Structure;

public class Namespace : ImmediateResolvable, IScope
{
    [SetsRequiredMembers]
    public Namespace(IScope parent, string name)
    {
        scope = new SimpleScope {Parent = parent};
        HoldingScope = parent;
        Name = name;
    }

    private readonly SimpleScope scope;

    IScope IScope.Parent => HoldingScope;


    public Result Define(IResolvable value, SourceRange range)
        => scope.Define(value, range);
    public Result<IResolvable> ResolveDirect(string identifier, SourceRange range, Context ctx, bool upwards = true)
        => scope.ResolveDirect(identifier, range, ctx, upwards);
    public Result<IFullNamed> Resolve(string identifier, SourceRange range, Context ctx, bool upwards = true)
        => scope.Resolve(identifier, range, ctx, upwards);

    public IEnumerable<IResolvable> Items(Context ctx)
        => scope.Items(ctx);

    public static string TypeName => "namespace";
}


