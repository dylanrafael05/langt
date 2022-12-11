using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record IndexExpression(ASTNode Value, ASTToken Open, ASTNode IndexValue, ASTToken Close) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Value, Open, IndexValue, Close};

    public override bool IsLValue => true;

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Indexing . . .");
        visitor.Visit(Value);
        visitor.PutString(". . . with index . . .");
        visitor.Visit(IndexValue);
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        // TODO: should this be raw or not vvvvvv
        Value.TypeCheck(generator);
        IndexValue.TypeCheck(generator);

        if(!Value.TransformedType.IsPointer)
        {
            generator.Diagnostics.Error($"Cannot index a non-pointer", Range);
        }

        if(!generator.MakeMatch(LangtType.Int64, IndexValue))
        {
            generator.Diagnostics.Error($"Cannot index with a non-integral index", Range);
        }

        ExpressionType = Value.TransformedType;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Value.Lower(lowerer);
        var val = lowerer.PopValue();

        IndexValue.Lower(lowerer);
        var index = lowerer.PopValue();

        var pointeeType = lowerer.LowerType(Value.TransformedType.PointeeType!);

        lowerer.PushValue(
            val.Type,
            lowerer.Builder.BuildGEP2(pointeeType, val.LLVM, new[] {index.LLVM}, "index." + val.Type.Name)
        );
    }
}
