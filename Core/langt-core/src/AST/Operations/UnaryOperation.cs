using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

using TT = Langt.Lexing.TokenType;

namespace Langt.AST;

public record UnaryOperation(ASTToken Operator, ASTNode Operand) : ASTNode, IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {Operator, Operand};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString($"Unary {Operator.ContentStr}");
        visitor.Visit(Operand);
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Operand.TypeCheck(generator);
        
        if(Operator.Type is not TT.Not) throw new Exception("Unknown unary operator " + Operator.Type);

        if(!generator.MakeMatch(Codegen.LangtType.Bool, Operand))
        {
            generator.Diagnostics.Error("Cannot negate non-boolean value", Range);
        }
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Operand.Lower(lowerer);
        var o = lowerer.PopValue();

        lowerer.PushValue(o.Type, lowerer.Builder.BuildNot(o.LLVM, "not"));
    }
}
