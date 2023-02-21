using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStruct(ASTToken Struct, ASTToken Name, GenericParameterSpecification? Generic, ASTToken Open, SeparatedCollection<DefineStructField> Fields, ASTToken Close) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Struct, Name, Generic, Open, Fields, Close};

    public LangtNamedStructureType? StructureType {get; private set;}

    public override Result HandleDefinitions(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();

        var w = new WeakResolution
        (
            new ResolutionIdentifier(Name.ContentStr, state.CTX.ResolutionScope),

            w => 
            {
                var builder = ResultBuilder.Empty();

                var typeScope = new SimpleScope {Parent = w.Identifier.HoldingScope};
                var genericTypes = new List<LangtType>();

                foreach(var spec in Generic?.TypeSpecs.Values.OrEmpty()!)
                {
                    var gen = new LangtGenericParameterType(spec.ContentStr, w.Identifier.HoldingScope) {DefinitionRange = spec.Range};

                    var newty = typeScope.Define(WeakResolution.Wrapping(gen), spec.Range);
                    builder.AddData(newty);

                    if(newty) 
                        genericTypes.Add(gen);
                }

                if(!builder) 
                    return builder.BuildError<IResolvable>();

                var sb = new StructureTypeBuilder(typeScope)
                    .WithGenericTypes(genericTypes.ToArray());

                foreach(var f in Fields.Values) 
                {
                    var sfr = f.Field(state);
                    builder.AddData(sfr);

                    if(!sfr) 
                    {
                        state.CTX.RestoreScope();
                        return builder.BuildError<IResolvable>();
                    }

                    var sf = sfr.Value;

                    if(StructureType!.HasField(sf.Name))
                    {
                        state.CTX.RestoreScope();
                        return builder.WithDgnError($"Cannot redefine field {sf.Name}", Range)
                            .BuildError<IResolvable>();
                    }

                    sb.AddField(sf.Name, sf.Ty);
                }

                return Result.Success<IResolvable>
                (
                    new LangtNamedStructureType()
                );
            }
        );

        var dt = state.CTX.ResolutionScope.Define
        (
            s => 
            {
                var typeScope = new LangtScope(s);
                var genericTypes = new List<LangtType>();

                foreach(var spec in EnumerableExtensions.OrEmpty(Generic?.TypeSpecs.Values))
                {
                    var newty = typeScope.Define(s => new LangtGenericParameterType(spec.ContentStr, s) {DefinitionRange = spec.Range}, spec.Range);
                    builder.AddData(newty);

                    if(newty) genericTypes.Add(newty.Value);
                }

                return new LangtNamedStructureType(Name.ContentStr, s, typeScope, genericTypes)
                {
                    DefinitionRange = Range,
                    Documentation = Struct.Documentation
                };
            }, 

            Range,
            Name.Range,

            builder,

            out var t
        );

        builder.AddData(dt);

        if(!dt) return dt;

        StructureType = t;

        return builder.Build();
    }

    public override Result RefineDefinitions(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();

        if(StructureType is null) return Result.Error(SilentError.Create());

        state.CTX.RedirectScope(StructureType.TypeScope);

        foreach(var f in Fields.Values) 
        {
            var sfr = f.Field(state);
            builder.AddData(sfr);

            if(!sfr) 
            {
                state.CTX.RestoreScope();
                return builder.Build();
            }

            var sf = sfr.Value;

            if(StructureType!.HasField(sf.Name))
            {
                state.CTX.RestoreScope();
                return builder.WithDgnError($"Cannot redefine field {sf.Name}", Range).Build();
            }

            StructureType.AddField(sf.Name, sf.Ty);
        }

        state.CTX.RestoreScope();
        return builder.Build();
    }
}