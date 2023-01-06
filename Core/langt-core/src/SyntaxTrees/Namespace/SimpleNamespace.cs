using Langt.Codegen;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record SimpleNamespace(ASTToken Name) : ASTNamespace
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name};

    public override Result<LangtNamespace> Resolve(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        var r = ResolveFrom(state.CG.Project.GlobalScope, Name.ContentStr, options.AllowNamesapceDefinitions);
        builder.AddData(r);
        if(!r) return builder.Build<LangtNamespace>();

        builder.AddStaticReference(Name.Range, r.Value);
        return builder.Build(r.Value);
    }
}
