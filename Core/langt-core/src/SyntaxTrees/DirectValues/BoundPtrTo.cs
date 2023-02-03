using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundPtrTo(UnaryOperation Source, BoundASTNode Value) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
    public override LangtType Type => LangtPointerType.Create(Value.Type.ElementType!).Expect("Invalid pointer type");
}
