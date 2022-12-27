using Langt.Codegen;

namespace Langt.AST;

public record DefineAlias(ASTToken Alias, ASTToken Name, ASTToken Eq, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Alias, Name, Eq, Type};

    public LangtAliasType? AliasType {get; private set;}

    public override Result HandleDefinitions(ASTPassState state)
    {
        var t = new LangtAliasType(Name.ContentStr);
        
        var dr = state.CG.ResolutionScope.DefineType(t, Range);
        if(!dr) return dr;

        AliasType = t;

        return Result.Success();
    }

    public override Result RefineDefinitions(ASTPassState state)
    {
        var tr = Type.Resolve(state);
        if(!tr) return tr.Drop();

        AliasType!.SetBase(tr.Value);

        return Result.Success();
    }
}
