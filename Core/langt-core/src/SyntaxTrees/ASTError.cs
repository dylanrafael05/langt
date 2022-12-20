using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ASTInvalid(SourceRange ErrRange) : ASTNode
{
    public override ASTChildContainer ChildContainer => new();
    public override SourceRange Range => ErrRange;

    public override void Dump(VisitDumper visitor)
    {}
    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {}
}
