using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundFunctionBlockBody(FunctionBlockBody Source, BoundASTNode Body) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Body};
}

public record FunctionBlockBody(Block Block) : FunctionBody
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Block};

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var b = Block.Bind(state);
        if(!b) return b;

        return b.Map<BoundASTNode>(k => new BoundFunctionBlockBody(this, k));
    }
}