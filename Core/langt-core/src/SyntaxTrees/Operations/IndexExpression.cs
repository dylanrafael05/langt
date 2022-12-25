using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record IndexExpression(ASTNode Value, ASTToken Open, ASTNode IndexValue, ASTToken Close) : ASTNode
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Value, Open, IndexValue, Close};

    public override bool IsLValue => true;

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Indexing . . .");
        visitor.Visit(Value);
        visitor.PutString(". . . with index . . .");
        visitor.Visit(IndexValue);
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        // TODO: should this be raw or not vvvvvv
        Value.TypeCheck(state);
        IndexValue.TypeCheck(state);

        if(!Value.TransformedType.IsPointer)
        {
            state.Error($"Cannot index a non-pointer", Range);
        }

        if(!state.MakeMatch(LangtType.Int64, IndexValue))
        {
            state.Error($"Cannot index with a non-integral index", Range);
        }

        RawExpressionType = Value.TransformedType;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Value.Lower(lowerer);
        var val = lowerer.PopValue(DebugSourceName);

        IndexValue.Lower(lowerer);
        var index = lowerer.PopValue(DebugSourceName);

        var pointeeType = lowerer.LowerType(Value.TransformedType.PointeeType!);

        lowerer.PushValue( 
            val.Type,
            lowerer.Builder.BuildGEP2(pointeeType, val.LLVM, new[] {index.LLVM}, "index." + val.Type.Name),
            DebugSourceName
        );
    }
}
