using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundAssignment(Assignment Source, BoundASTNode Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};
}

public record Assignment(ASTNode Left, ASTToken Assign, ASTNode Right) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Assign, Right};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString($"Assignment");
        visitor.Visit(Left);
        visitor.Visit(Right);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        var leftResult = Left.Bind(state, new TypeCheckOptions {AutoDeferenceLValue = false});
        builder.AddData(leftResult);
        if(!leftResult) return builder.BuildError<BoundASTNode>();

        var left = leftResult.Value;
        
        if(!left.Type.IsReference)
        {
            return builder.WithDgnError($"Cannot assign to a non-assignable value", Range).BuildError<BoundASTNode>();
        }

        var rightResult = Right.BindMatchingExprType(state, left.Type.ElementType!);
        builder.AddData(rightResult);
        if(!rightResult) return builder.BuildError<BoundASTNode>();

        var right = rightResult.Value;
        
        return builder.Build<BoundASTNode>
        (
            new BoundAssignment(this, left, right)
            {
                Type = LangtType.None
            }
        );
    }
}
