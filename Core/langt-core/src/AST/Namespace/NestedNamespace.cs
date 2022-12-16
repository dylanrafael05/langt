using Langt.Codegen;

namespace Langt.AST;

public record NestedNamespace(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier) : ASTNamespace
{
    public override ASTChildContainer ChildContainer => new() {Namespace, Dot, Identifier};

    public override LangtNamespace? Resolve(ASTPassState state, bool allowDefinitions = false)
    {
        var ns = Namespace.Resolve(state, allowDefinitions);
        if(ns is null) return null;

        return ResolveFrom(ns, Identifier.ContentStr, state, allowDefinitions);
    }

}
