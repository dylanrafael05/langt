using Langt.Structure;
using Langt.Structure.Resolutions;

namespace Langt.AST;

public record BoundStructFieldAccess(DotAccess SourceNode, BoundASTNode Left) : BoundASTNode(SourceNode)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left};

    public required LangtStructureField Field {get; init;}

    public override LangtType Type => Field.Type;
    public override bool IsAssignable => Left.IsAssignable;
}
