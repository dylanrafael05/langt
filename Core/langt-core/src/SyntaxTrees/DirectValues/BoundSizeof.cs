using Langt.Structure;

namespace Langt.AST;

public record BoundSizeof(Sizeof Source, LangtType SizeofType) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};

    public override LangtType Type => LangtType.UIntSZ;
}
