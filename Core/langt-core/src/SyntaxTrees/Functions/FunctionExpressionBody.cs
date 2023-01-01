using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundFunctionExpressionBody(FunctionExpressionBody Source, BoundASTNode Expression) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Expression};

    public override void LowerSelf(CodeGenerator generator)
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
        var e = Expression.Bind(state);
        if(!e) return e;

        return Result.Success<BoundASTNode>
        (
            new BoundFunctionExpressionBody(this, e.Value)
            {
                RawExpressionType = e.Value.TransformedType, 
                Returns = true
            }
        );
    }
}