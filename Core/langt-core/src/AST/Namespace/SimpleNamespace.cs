using Langt.Codegen;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record SimpleNamespace(ASTToken Name) : ASTNamespace
{
    public override ASTChildContainer ChildContainer => new() {Name};

    public override LangtNamespace? Resolve(CodeGenerator generator, [NotNullWhen(true)] bool allowDefinitions = false)
        => ResolveFrom(generator.Project.GlobalScope, Name.ContentStr, generator, allowDefinitions);
}
