using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record FunctionExpressionBody(ASTToken Eq, ASTNode Expression) : FunctionBody
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Eq, Expression};
    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("returns...");
        visitor.Visit(Expression);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Expression.Bind(state);
}
