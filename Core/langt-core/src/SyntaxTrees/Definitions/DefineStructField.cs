using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Resolutions;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStructField(ASTToken Name, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name, Type};

    // TODO: move resolution logic to TypeCheckRaw?
    public FieldSymbol Field(Context ctx) 
        => new(Name.ContentStr, Type.GetSymbol(ctx), Range);
}
