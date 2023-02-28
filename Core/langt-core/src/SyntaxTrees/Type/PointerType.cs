using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record PointerType(ASTToken Ptr, ASTType Type) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Ptr, Type};

    public override ISymbol<LangtType> GetSymbol(Context ctx)
        => new PointerTypeSymbol {ElementType = Type.GetSymbol(ctx)};
}