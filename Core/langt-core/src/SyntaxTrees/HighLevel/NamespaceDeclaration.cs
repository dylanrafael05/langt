using Langt.Codegen;

namespace Langt.AST;

public record NamespaceDeclaration(ASTToken Namespace, ASTNamespace Identifier) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Namespace, Identifier};

    public LangtNamespace? LNamespace {get; set;}

    public override Result HandleDefinitions(ASTPassState state)
    {
        var n = Identifier.Resolve(state, new TypeCheckOptions {AllowNamesapceDefinitions=true});
        if(!n) return n.Drop();

        LNamespace = n.Value;

        state.CG.SetCurrentNamespace(LNamespace);

        return Result.Success();
    }

    public override Result RefineDefinitions(ASTPassState state)
    {
        state.CG.SetCurrentNamespace(LNamespace!);

        return Result.Success();
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        state.CG.SetCurrentNamespace(LNamespace!);

        return Result.Success<BoundASTNode>(new BoundASTWrapper(this));
    }
}
