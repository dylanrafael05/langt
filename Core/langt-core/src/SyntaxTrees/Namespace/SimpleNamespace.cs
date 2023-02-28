using Langt.Structure;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record SimpleNamespace(ASTToken Name) : ASTNamespace
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name};

    public override ISymbol<Namespace> GetSymbol(Context ctx)
        => ctx.Project.GlobalScope.ResolveSymbol(Name.ContentStr, Name.Range).As<Namespace>();
}
