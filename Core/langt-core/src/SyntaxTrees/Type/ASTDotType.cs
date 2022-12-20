using Langt.Codegen;

namespace Langt.AST;

public record DotType(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier): ASTType
{
    public override ASTChildContainer ChildContainer => new() {Namespace, Dot, Identifier};

    public override LangtType? Resolve(ASTPassState state)
    {
        var ns = Namespace.Resolve(state);

        if(ns is null) return null;

        return ns.ResolveType(Identifier.ContentStr, Range, state);
    }
}   