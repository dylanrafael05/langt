using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStructField(ASTToken Name, ASTType Type) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Name, Type};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString(Name.ContentStr);
        visitor.Visit(Type);
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        ExpressionType = LangtType.None;
    }

    // TODO: move resolution logic to TypeCheckRaw?
    public LangtStructureField? Field(CodeGenerator generator) 
    {
        var t = Type.Resolve(generator);
        if(t is null) return null;

        return new(Name.ContentStr, t);
    }
}
