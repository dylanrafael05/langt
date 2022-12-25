using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record FunctionExpressionBody(ASTToken Eq, ASTNode Expression) : FunctionBody
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Eq, Expression};
    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("returns...");
        visitor.Visit(Expression);
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        Expression.TypeCheck(state);
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Expression.Lower(lowerer);
    }
}
