using Langt.Structure;

namespace Langt.AST;

public record NamespaceDeclaration(ASTToken NamespaceTok, ASTNamespace Ns) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {NamespaceTok, Ns};

    public Namespace? Namespace {get; set;}

    public override Result HandleDefinitions(Context ctx)
    {
        var baseNs = Ns.GetSymbol(ctx);
        
        var n = baseNs.As<IScope>("scope");
        var names = new Stack<string>();

        while(n is ResolutionSymbol ressym)
        {
            names.Push(ressym.Name);
            n = ressym.SearchScope;
        }

        names.Push(((DirectResolutionSymbol)n).Name);

        IScope scope = ctx.Project.GlobalScope;
        foreach(var name in names.Reverse())
        {
            var ns = new Namespace(scope, name);
            scope.Define(ns, Range);
            scope = ns;
        }

        Namespace = (Namespace)scope;

        ctx.SetCurrentNamespace(Namespace);

        return Result.Success();
    }

    public override Result RefineDefinitions(Context ctx)
    {
        if(Namespace is not null) ctx.SetCurrentNamespace(Namespace!);

        return Result.Success();
    }

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        if(Namespace is not null) ctx.SetCurrentNamespace(Namespace!);

        return Result.Success<BoundASTNode>(new BoundEmpty(this));
    }
}
