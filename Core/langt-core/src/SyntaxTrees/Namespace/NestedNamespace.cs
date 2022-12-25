using Langt.Codegen;

namespace Langt.AST;

public record NestedNamespace(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier) : ASTNamespace
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Namespace, Dot, Identifier};

    public override Result<LangtNamespace> Resolve(ASTPassState state, bool allowDefinitions = false)
    {
        var ns = Namespace.Resolve(state, allowDefinitions);
        if(!ns) return ns;

        return ResolveFrom(ns.Value, Identifier.ContentStr, state, allowDefinitions).WithDataFrom(ns);
    }

}
