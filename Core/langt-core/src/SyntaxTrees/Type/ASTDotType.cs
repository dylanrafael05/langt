using Langt.Structure;


namespace Langt.AST;

public record NestedType(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier): ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Namespace, Dot, Identifier};

    public override ISymbol<LangtType> GetSymbol(Context ctx)
        => Namespace.GetSymbol(ctx).ResolveSymbol(Identifier.ContentStr, Identifier.Range).As<LangtType>("type");
}   