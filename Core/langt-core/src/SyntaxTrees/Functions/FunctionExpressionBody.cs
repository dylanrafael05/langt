using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundFunctionExpressionBody(FunctionExpressionBody Source, BoundASTNode Expression) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Expression};

    public override void LowerSelf(Context generator)
    {
        Expression.Lower(generator);
        generator.Builder.BuildRet(generator.PopValue(DebugSourceName).LLVM);
    }
}

public record FunctionExpressionBody(ASTToken Eq, ASTNode Expression) : FunctionBody
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Eq, Expression};
    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("returns...");
        visitor.Visit(Expression);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var e = Expression.BindMatchingExprType(state, options.TargetType!);
        if(!e) return e;

        return e.Map<BoundASTNode>
        (
            k => new BoundFunctionExpressionBody(this, k)
            {
                Type = k.Type, 
                Returns = true
            }
        );
    }
}
