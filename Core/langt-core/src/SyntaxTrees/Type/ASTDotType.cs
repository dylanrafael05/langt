using Langt.Codegen;

namespace Langt.AST;

public record DotType(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier): ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Namespace, Dot, Identifier};

    public override Result<LangtType> Resolve(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();

        var nr = Namespace.Resolve(state);
        builder.AddData(nr);
        if(!nr) return builder.Build<LangtType>();

        var ns = nr.Value;
        var tr = ns!.ResolveType(Identifier.ContentStr, Range);
        builder.AddData(tr);
        if(!tr) return builder.Build<LangtType>();

        builder.AddStaticReference(Identifier.Range, tr.Value);
        return builder.Build(tr.Value);
    }
}   