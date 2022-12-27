using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record FunctionBlockBody(Block Block) : FunctionBody
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Block};

    public override void Dump(VisitDumper visitor)
    {
        visitor.Visit(Block);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Block.Bind(state);
}