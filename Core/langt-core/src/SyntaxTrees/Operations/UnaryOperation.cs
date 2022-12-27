using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

using TT = Langt.Lexing.TokenType;

namespace Langt.AST;

public record UnaryOperation(ASTToken Operator, ASTNode Operand) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Operator, Operand};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString($"Unary {Operator.ContentStr}");
        visitor.Visit(Operand);
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        Operand.TypeCheck(state);
        
        if(Operator.Type is not TT.Not) throw new Exception("Unknown unary operator " + Operator.Type);

        if(!state.MakeMatch(LangtType.Bool, Operand))
        {
            state.Error("Cannot negate non-boolean value", Range);
        }
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Operand.Lower(lowerer);
        var o = lowerer.PopValue(DebugSourceName);

        lowerer.PushValue(o.Type, lowerer.Builder.BuildNot(o.LLVM, "not"), DebugSourceName);
    }
}
