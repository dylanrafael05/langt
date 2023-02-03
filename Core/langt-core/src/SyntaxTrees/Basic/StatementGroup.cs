using Langt.Structure;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

public record StatementGroup(IList<ASTNode> Statements) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Statements};
    public override bool BlockLike => true;

    public override Result HandleDefinitions(ASTPassState state)
        => ResultGroup.GreedyForeach(Statements, n => n.HandleDefinitions(state)).Combine();
    public override Result RefineDefinitions(ASTPassState state)
        => ResultGroup.GreedyForeach(Statements, n => n.RefineDefinitions(state)).Combine();
    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => BoundGroup.BindFromNodes(this, Statements, state);
}
