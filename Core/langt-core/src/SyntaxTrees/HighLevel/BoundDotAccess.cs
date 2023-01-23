using Langt.Structure;
using Langt.Structure.Resolutions;

namespace Langt.AST;

public record BoundStructFieldAccess(DotAccess SourceNode, BoundASTNode Left) : BoundASTNode(SourceNode)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left};

    public LangtStructureField? Field {get; set;}
    public int? FieldIndex {get; set;}

    public override LangtType Type => LangtReferenceType.Create(Field!.Type).Expect();
}
