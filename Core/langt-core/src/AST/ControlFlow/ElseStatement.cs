using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ElseStatement(ASTToken Else, ASTNode End) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Else, End};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Else");
        visitor.Visit(End);
    }

    public override void TypeCheckSelf(CodeGenerator generator)
    {
        End.TypeCheck(generator);
        Returns = End.Returns;
        
        RawExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        End.Lower(lowerer);
    }
}
