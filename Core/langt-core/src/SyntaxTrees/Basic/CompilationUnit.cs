using Langt.Structure;

namespace Langt.AST;

public record CompilationUnit(StatementGroup Group) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Group};

    public override bool BlockLike => true;

    
    public override Result HandleDefinitions(Context ctx)
        => Group.HandleDefinitions(ctx);
    public override Result RefineDefinitions(Context ctx)
        => Group.RefineDefinitions(ctx);
    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
        => Group.Bind(ctx);
}
