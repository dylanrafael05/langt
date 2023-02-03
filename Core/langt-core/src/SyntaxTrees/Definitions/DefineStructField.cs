using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStructField(ASTToken Name, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name, Type};

    // TODO: move resolution logic to TypeCheckRaw?
    public Result<LangtStructureField> Field(ASTPassState state) 
    {
        var t = Type.Resolve(state);
        if(!t) return t.ErrorCast<LangtStructureField>();

        return Result.Success(new LangtStructureField(Name.ContentStr, t.Value)).WithDataFrom(t);
    }
}
