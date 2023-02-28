using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStruct(ASTToken Struct, ASTToken Name, GenericParameterSpecification? Generic, ASTToken Open, SeparatedCollection<DefineStructField> Fields, ASTToken Close) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Struct, Name, Generic, Open, Fields, Close};

    public LangtNamedStructureType? StructureType {get; private set;}

    public override Result HandleDefinitions(Context ctx)
    {
        var scope = ctx.ResolutionScope;
        var typeScope = new SimpleScope {Parent = scope};

        var genericParams = Generic?.TypeSpecs?.Values?.Select(
            a => new LangtGenericParameterType(a.ContentStr, typeScope)
            {
                DefinitionRange = a.Range
            }
        ) ?? Enumerable.Empty<LangtGenericParameterType>();

        ctx.RedirectScope(typeScope);
        var result = scope.Define(
            new LangtNamedStructureType(
                Name.ContentStr, 
                Fields.Values.Select(f => f.Field(ctx)), 
                scope, 
                typeScope, 
                genericParams.Cast<LangtType>().ToList()
            )
            {
                DefinitionRange = Name.Range
            },
            Name.Range
        );
        ctx.CloseScope();

        return result;
    }
}