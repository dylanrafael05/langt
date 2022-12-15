using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record FunctionExpressionBody(ASTToken Eq, ASTNode Expression) : FunctionBody
{
    public override ASTChildContainer ChildContainer => new() {Eq, Expression};
    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("returns...");
        visitor.Visit(Expression);
    }

    public override void TypeCheckSelf(CodeGenerator generator)
    {
        Expression.TypeCheck(generator);
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Expression.Lower(lowerer);
    }
}
