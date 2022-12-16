using Langt.Codegen;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record SimpleNamespace(ASTToken Name) : ASTNamespace
{
    public override ASTChildContainer ChildContainer => new() {Name};

    public override LangtNamespace? Resolve(ASTPassState state, [NotNullWhen(true)] bool allowDefinitions = false)
        => ResolveFrom(state.CG.Project.GlobalScope, Name.ContentStr, state, allowDefinitions);
}
