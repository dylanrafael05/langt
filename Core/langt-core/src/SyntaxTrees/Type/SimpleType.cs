using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;


namespace Langt.AST;

public record SimpleType(ASTToken Name) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name};

    public override ISymbol<LangtType> GetSymbol(Context ctx)
        => ctx.ResolutionScope.ResolveSymbol(Name.ContentStr, Range).As<LangtType>("type");
}
