using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundIndexExpression(IndexExpression Source, BoundASTNode Value, BoundASTNode Index) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value, Index};

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Value.Lower(lowerer);
        var val = lowerer.PopValue(DebugSourceName);

        Index.Lower(lowerer);
        var index = lowerer.PopValue(DebugSourceName);

        var pointeeType = lowerer.LowerType(Value.Type.ElementType!);

        lowerer.PushValue( 
            val.Type,
            lowerer.Builder.BuildGEP2(pointeeType, val.LLVM, new[] {index.LLVM}, "index." + val.Type.Name),
            DebugSourceName
        );
    }
}

public record IndexExpression(ASTNode Value, ASTToken Open, ASTNode IndexValue, ASTToken Close) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Value, Open, IndexValue, Close};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Indexing . . .");
        visitor.Visit(Value);
        visitor.PutString(". . . with index . . .");
        visitor.Visit(IndexValue);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        var results = Result.All
        (
            Value.Bind(state),
            IndexValue.BindMatchingExprType(state, LangtType.Int64)
        );
        builder.AddData(results);

        if(!results) return builder.BuildError<BoundASTNode>();

        var (val, index) = results.Value;

        if(!val.Type.IsPointer)
        {
            return builder.WithDgnError("Cannot index a non-pointer", Range).BuildError<BoundASTNode>();
        }

        return builder.Build<BoundASTNode>
        (
            new BoundIndexExpression(this, val, index) 
            {
                Type = LangtReferenceType.Create(val.Type.ElementType).Expect()
            }
        );
    }
}
