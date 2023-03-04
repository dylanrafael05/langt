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

        var builder = ResultBuilder.Empty();

        var genericParams = Generic?.TypeSpecs?.Values?.Select(
            a => {
                var r = new LangtGenericParameterType(a.ContentStr, typeScope)
                {
                    DefinitionRange = a.Range
                };
                builder.AddData(typeScope.Define(r, a.Range));
                return r;
            }
        ) ?? Enumerable.Empty<LangtGenericParameterType>();

        ctx.RedirectScope(typeScope);
            StructureType = new LangtNamedStructureType
            (
                Name.ContentStr, 
                Fields.Values.Select(f => f.Field(ctx)).ToArray(),
                scope, 
                typeScope, 
                genericParams.Cast<LangtType>().ToList()
            )
            {
                DefinitionRange = Name.Range
            };
            var result = scope.Define(StructureType, Name.Range);
            builder.AddData(result);
        ctx.CloseScope();

        if(!builder) StructureType = null;

        return builder.Build();
    }

    public override Result RefineDefinitions(Context ctx)
        => StructureType?.Complete(ctx) ?? Result.Success();
}