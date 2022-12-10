using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record SimpleType(ASTToken Name) : ASTType
{
    public override ASTChildContainer ChildContainer => new() {Name};

    public override void Dump(VisitDumper visitor)
    {
        visitor.VisitNoDepth(Name);
    }

    public override LangtType? Resolve(CodeGenerator context)
        => context.ResolutionScope.ResolveType(Name.ContentStr, Range, context.Diagnostics);
}
