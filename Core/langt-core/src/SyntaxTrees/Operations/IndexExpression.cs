using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundIndexExpression(IndexExpression Source, BoundASTNode Value, BoundASTNode Index) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value, Index};
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
