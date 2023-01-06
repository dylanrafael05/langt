using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundFunctionBlockBody(FunctionBlockBody Source, BoundASTNode Body) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Body};

    public override void LowerSelf(CodeGenerator generator)
    {
        Body.Lower(generator);

        if(generator.CurrentFunction!.Type.ReturnType == LangtType.None && !Body.Returns)
        {
            generator.Builder.BuildRetVoid();
        }
    }
}

public record FunctionBlockBody(Block Block) : FunctionBody
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Block};

    public override void Dump(VisitDumper visitor)
    {
        visitor.Visit(Block);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var b = Block.Bind(state);
        if(!b) return b;

        return b.Map<BoundASTNode>(k => new BoundFunctionBlockBody(this, k));
    }
}