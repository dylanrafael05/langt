using Langt.Structure;

namespace Langt.AST;

public record BoundDereference(UnaryOperation Source, BoundASTNode Value) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
    public override LangtType Type => Value.Type.ElementType!;

    public override bool IsAssignable => true;
}