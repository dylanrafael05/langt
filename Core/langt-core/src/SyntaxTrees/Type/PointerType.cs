using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record PointerType(ASTToken Ptr, ASTType Type) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Ptr, Type};
    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("pointer to");
        visitor.Visit(Type);
    }

    public override Result<LangtType> Resolve(ASTPassState state)
        => Type.Resolve(state).Map(t => LangtPointerType.Create(t, Range)).As<LangtType>();
}