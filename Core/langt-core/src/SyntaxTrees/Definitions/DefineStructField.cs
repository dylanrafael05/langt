using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Resolutions;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStructField(ASTToken Name, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name, Type};

    // TODO: move resolution logic to TypeCheckRaw?
    public Result<(string Name, LangtType Ty)> Field(ASTPassState state) 
    {
        var t = Type.Resolve(state);
        if(!t) return t.ErrorCast<(string, LangtType)>();

        if(!t.Value.IsConstructed) return t
            .WithError(Diagnostic.Error($"Cannot have a field of the unconstructed type {t.Value}", Range))
            .ErrorCast<(string, LangtType)>();

        return Result.Success((Name.ContentStr, t.Value)).WithDataFrom(t);
    }
}
