using Langt.Structure;

namespace Langt.AST;

public record DefineAlias(ASTToken Alias, ASTToken Name, ASTToken Eq, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Alias, Name, Eq, Type};

    public LangtAliasType? AliasType {get; private set;}

    public override Result HandleDefinitions(Context ctx)
        => ctx.ResolutionScope.Define(
            new LangtAliasType(
                Name.ContentStr, 
                ctx.ResolutionScope, 
                Type.GetSymbol(ctx)
            )
            {
                DefinitionRange = Range,
                Documentation = Alias.Documentation
            },
            Range
        );
}
