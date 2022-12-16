using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ParentheticExpression(ASTToken Open, ASTNode Value, ASTToken End) : ASTNode(), IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {Open, Value, End};

    public override void Dump(VisitDumper visitor)
        => visitor.VisitNoDepth(Value);

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        Value.TypeCheck(state);
        RawExpressionType = Value.TransformedType;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Value.Lower(lowerer);
    }
}
