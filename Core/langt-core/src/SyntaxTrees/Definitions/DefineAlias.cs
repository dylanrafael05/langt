using Langt.Codegen;

namespace Langt.AST;

public record DefineAlias(ASTToken Alias, ASTToken Name, ASTToken Eq, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Alias, Name, Eq, Type};

    public LangtAliasType? AliasType {get; private set;}

    public override Result HandleDefinitions(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();
        
        var dr = state.CG.ResolutionScope.Define
        (
            s => new LangtAliasType(Name.ContentStr, s) 
            {
                Documentation = Alias.Documentation,
                DefinitionRange = Range
            }, 
            
            Range, 
            Name.Range, 
            
            builder,
            
            out var t
        );

        builder.AddData(dr);
        if(!dr) return dr;
        
        AliasType = t;

        return Result.Success();
    }

    public override Result RefineDefinitions(ASTPassState state)
    {
        var tr = Type.Resolve(state);
        if(!tr) return tr.Drop();

        if(AliasType is null) return Result.Error(SilentError.Create());

        AliasType.SetBase(tr.Value);

        return Result.Success();
    }
}
