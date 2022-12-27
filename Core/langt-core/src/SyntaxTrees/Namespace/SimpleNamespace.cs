using Langt.Codegen;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record SimpleNamespace(ASTToken Name) : ASTNamespace
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name};

    public override Result<LangtNamespace> Resolve(ASTPassState state, [NotNullWhen(true)] bool allowDefinitions = false)
        => ResolveFrom(state.CG.Project.GlobalScope, Name.ContentStr, state, allowDefinitions);
}
