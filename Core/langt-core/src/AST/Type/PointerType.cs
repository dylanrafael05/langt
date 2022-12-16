using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record PointerType(ASTToken Ptr, ASTType Type) : ASTType
{
    public override ASTChildContainer ChildContainer => new() {Ptr, Type};
    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("pointer to");
        visitor.Visit(Type);
    }

    public override LangtType? Resolve(ASTPassState state)
    {
        var t = Type.Resolve(state);
        if(t is null) return null;
        return LangtType.PointerTo(t);
    }
}