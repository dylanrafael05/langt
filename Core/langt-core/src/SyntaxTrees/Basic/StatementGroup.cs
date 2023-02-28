using Langt.Structure;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

public record StatementGroup(IList<ASTNode> Statements) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Statements};
    public override bool BlockLike => true;

    public override Result HandleDefinitions(Context ctx)
        => ResultGroup.GreedyForeach(Statements, n => n.HandleDefinitions(ctx)).Combine();
    public override Result RefineDefinitions(Context ctx)
        => ResultGroup.GreedyForeach(Statements, n => n.RefineDefinitions(ctx)).Combine();
    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
        => BoundGroup.BindFromNodes(this, Statements, ctx);
}
