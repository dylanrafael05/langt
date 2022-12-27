using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundAssignment(Assignment Source, BoundASTNode Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};

    public override void LowerSelf(CodeGenerator generator)
    {
        Left.Lower(generator);
        Right.Lower(generator);

        var (right, left) = (generator.PopValue(DebugSourceName), generator.PopValue(DebugSourceName));

        generator.Builder.BuildStore(right.LLVM, left.LLVM);
    }
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
        var leftResult = Left.Bind(state, new TypeCheckOptions {AutoDeferenceLValue = false});
        if(!leftResult) return leftResult;

        var left = leftResult.Value;

        var rightResult = Right.BindMatching(state, left.TransformedType.PointeeType!);
        if(!rightResult) return rightResult;
        var right = rightResult.Value;

        var builder = ResultBuilder.From(leftResult, rightResult);

        if(!left.TransformedType.IsPointer || !left.IsLValue)
        {
            return builder.WithDgnError($"Cannot assign to a non-assignable value", Range).Build<BoundASTNode>();
        }
        
        return builder.Build<BoundASTNode>
        (
            new BoundAssignment(this, left, right)
            {
                RawExpressionType = LangtType.None
            }
        );
    }
}
