using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;


namespace Langt.AST;

public record BoundFunctionExpressionBody(FunctionExpressionBody Source, BoundASTNode Expression, IScope Scope) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Expression};
}

public record FunctionExpressionBody(ASTToken Eq, ASTNode Expression) : FunctionBody
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Eq, Expression};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        Expect.NonNull(options.PredefinedBlockScope, "Cannot bind expression body without a provided scope");

        var e = Expression.BindMatchingExprType(ctx, options.TargetType!);
        if(!e) return e;

        return e.Map<BoundASTNode>
        (
            k => new BoundFunctionExpressionBody(this, k, options.PredefinedBlockScope!)
            {
                Type = k.Type, 
                Returns = true
            }
        );
    }
}
