using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;
using Langt.Structure.Resolutions;

namespace Langt.AST;

public record SimpleType(ASTToken Name) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name};

    public override void Dump(VisitDumper visitor)
    {
        visitor.VisitNoDepth(Name);
    }

    public override Result<LangtType> Resolve(ASTPassState state)
    {
        var t = state.CTX.ResolutionScope.ResolveType(Name.ContentStr, Range);
        if(!t) return t;

        if(t.Value is IResolution r) 
            t = t.AddStaticReference(Range, r);

        return t;
    }
}
