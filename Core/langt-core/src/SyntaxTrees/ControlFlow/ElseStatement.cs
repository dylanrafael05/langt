using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ElseStatement(ASTToken Else, ASTNode End) : ASTNode
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Else, End};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Else");
        visitor.Visit(End);
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        End.TypeCheck(state);
        Returns = End.Returns;
        
        RawExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        End.Lower(lowerer);
    }
}
