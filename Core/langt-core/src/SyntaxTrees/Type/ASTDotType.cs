using Langt.Structure;
using Langt.Structure.Resolutions;

namespace Langt.AST;

public record NestedType(ASTNamespace Namespace, ASTToken Dot, ASTToken Identifier): ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Namespace, Dot, Identifier};

    public override Result<LangtType> Resolve(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();

        var nr = Namespace.Resolve(state);
        builder.AddData(nr);
        if(!nr) return builder.BuildError<LangtType>();

        var ns = nr.Value;
        var tr = ns!.ResolveType(Identifier.ContentStr, Range);
        builder.AddData(tr);
        if(!tr) return builder.BuildError<LangtType>();

        if(tr.Value is IResolution r)
            builder.AddStaticReference(Identifier.Range, r);
        
        return builder.Build(tr.Value);
    }
}   