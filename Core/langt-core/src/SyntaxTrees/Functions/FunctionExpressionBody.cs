using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;
using Langt.Structure.Resolutions;

namespace Langt.AST;

public record BoundFunctionExpressionBody(FunctionExpressionBody Source, BoundASTNode Expression, IScope Scope) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Expression};
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
        Expect.NonNull(options.PredefinedBlockScope, "Cannot bind expression body without a provided scope");

        var e = Expression.BindMatchingExprType(state, options.TargetType!);
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
