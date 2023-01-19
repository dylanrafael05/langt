using Langt.Codegen;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record SimpleNamespace(ASTToken Name) : ASTNamespace
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name};

    public override Result<LangtNamespace> Resolve(ASTPassState state, TypeCheckOptions? optionsMaybe)
    {
        var options = optionsMaybe ?? new();

        var builder = ResultBuilder.Empty();

        var r = ResolveFrom(state.CG.Project.GlobalScope, Name.ContentStr, default, options.AllowNamespaceDefinitions);
        builder.AddData(r);
        if(!r) return builder.BuildError<LangtNamespace>();

        builder.AddStaticReference(Name.Range, r.Value);
        return builder.Build(r.Value);
    }
}
