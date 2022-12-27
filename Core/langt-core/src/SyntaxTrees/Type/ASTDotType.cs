using Langt.Codegen;

namespace Langt.AST;

public record DotType(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier): ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Namespace, Dot, Identifier};

    public override Result<LangtType> Resolve(ASTPassState state)
    {
        var ns = Namespace.Resolve(state);
        
        if(!ns) return ns.Cast<LangtType>();
        else    return ns.Value.ResolveType(Identifier.ContentStr, Range);
    }
}   