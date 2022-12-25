using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record SimpleType(ASTToken Name) : ASTType
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Name};

    public override void Dump(VisitDumper visitor)
    {
        visitor.VisitNoDepth(Name);
    }

    public override LangtType? Resolve(ASTPassState state)
        => state.CG.ResolutionScope.ResolveType(Name.ContentStr, Range, state);
}
