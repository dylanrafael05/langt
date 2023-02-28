using Langt.Structure;

namespace Langt.AST;

public record NestedNamespace(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier) : ASTNamespace
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Namespace, Dot, Identifier};
    
    public override ISymbol<Namespace> GetSymbol(Context ctx)
        => Namespace.GetSymbol(ctx).ResolveSymbol(Identifier.ContentStr, Identifier.Range).As<Namespace>();
}
