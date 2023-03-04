using Langt.AST;

namespace Langt.Structure;

public class FileScope : IScope
{
    public FileScope(LangtProject proj)
    {
        curScope = proj.GlobalScope;
    }

    public void SetDefScope(IScope scope)
        => curScope = scope;

    public void AddNamespace(Namespace ns) 
        => namespaces.Add(ns);

    private IScope curScope;
    private readonly List<Namespace> namespaces = new();

    public IScope? Parent => curScope;

    public IEnumerable<IResolvable> Items(Context ctx)
        => curScope.Items(ctx);
    public Result<IFullNamed> Resolve(string identifier, SourceRange range, Context ctx, bool upwards = true)
        => curScope.Resolve(identifier, range, ctx, upwards);
    public Result Define(IResolvable value, SourceRange range)
        => curScope.Define(value, range);

    public Result<IResolvable> ResolveDirect(string identifier, SourceRange range, Context ctx, bool upwards = true)
    {
        var r = curScope.ResolveDirect(identifier, range, ctx, upwards);

        if(!r) 
        {
            foreach(var n in namespaces)
            {
                r = n.ResolveDirect(identifier, range, ctx, upwards);
                if(r) break;
            }
        }

        return r;
    }
}