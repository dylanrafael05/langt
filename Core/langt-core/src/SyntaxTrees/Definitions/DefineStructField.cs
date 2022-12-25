using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStructField(ASTToken Name, ASTType Type) : ASTNode
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Name, Type};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString(Name.ContentStr);
        visitor.Visit(Type);
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        RawExpressionType = LangtType.None;
    }

    // TODO: move resolution logic to TypeCheckRaw?
    public LangtStructureField? Field(ASTPassState state) 
    {
        var t = Type.Resolve(state);
        if(t is null) return null;

        return new(Name.ContentStr, t);
    }
}
