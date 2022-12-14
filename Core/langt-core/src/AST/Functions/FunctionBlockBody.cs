using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record FunctionBlockBody(Block Block) : FunctionBody
{
    public override ASTChildContainer ChildContainer => new() {Block};

    public override void Dump(VisitDumper visitor)
    {
        visitor.Visit(Block);
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Block.TypeCheck(generator);
        Returns = Block.Returns;
        RawExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Block.Lower(lowerer);
    }
}