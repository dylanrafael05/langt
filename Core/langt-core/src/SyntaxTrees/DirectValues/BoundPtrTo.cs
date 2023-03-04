using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundPtrTo(UnaryOperation Source, BoundASTNode Value) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
    public override LangtType Type => new LangtPointerType(Value.Type.ElementType!);
}
