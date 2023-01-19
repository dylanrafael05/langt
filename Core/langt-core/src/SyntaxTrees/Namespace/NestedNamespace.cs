using Langt.Codegen;

namespace Langt.AST;

public record NestedNamespace(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier) : ASTNamespace
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Namespace, Dot, Identifier};

    public override Result<LangtNamespace> Resolve(ASTPassState state, TypeCheckOptions? optionsMaybe)
    {
        var options = optionsMaybe ?? new();

        var builder = ResultBuilder.Empty();

        var ns = Namespace.Resolve(state, options);
        builder.AddData(ns);
        if(!ns) return builder.BuildError<LangtNamespace>();

        var r = ResolveFrom(ns.Value, Identifier.ContentStr, Identifier.Range, options.AllowNamespaceDefinitions);
        builder.AddData(r);
        if(!r) return builder.BuildError<LangtNamespace>();

        var res = r.Value;
        builder.AddStaticReference(Identifier.Range, res);

        return builder.Build(res);
    }
}
