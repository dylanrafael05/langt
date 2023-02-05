using Langt.Structure;

namespace Langt.AST;

public record ConsGenericStructType(ASTType Type, ASTToken GenTok, ASTToken Open, SeparatedCollection<ASTType> Arguments, ASTToken Close) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Type, GenTok, Open, Arguments, Close};

    public override Result<LangtType> Resolve(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();

        var argASTVals = Arguments.Values.ToArray();

        var baseTyRes = Type.Resolve(state);
        builder.AddData(baseTyRes);
        if(!builder) return builder.BuildError<LangtType>();

        var baseTy = baseTyRes.Value;
        
        var bparamsRes = ResultGroup.GreedyForeach(Arguments.Values, t => t.Resolve(state)).Combine();
        builder.AddData(bparamsRes);
        if(!builder) return builder.BuildError<LangtType>();

        var bparamTys = bparamsRes.Value.ToArray();
        var bparams = bparamTys.Zip(Arguments.Values.Select(k => k.Range)).Select(t => (ty: t.First, range: t.Second)).ToArray();

        if(!baseTy.IsStructure)
        {
            return builder.WithDgnError($"Cannot construct a generic type from {baseTy.FullName}; it is not a struct", Range)
                .BuildError<LangtType>();
        }

        if(baseTy.IsConstructed)
        {
            return builder.WithDgnError($"Cannot construct a generic type from {baseTy.FullName}; it is not a generic struct", Range)
                .BuildError<LangtType>();
        }

        if(argASTVals.Length != baseTy.GenericParameters.Count)
        {
            return builder.WithDgnError($"Cannot construct generic type from {baseTy.FullName} with arguments {bparams.Stringify(t => t.ty.FullName)}; expected {baseTy.GenericParameters.Count} arguments, not {bparams.Length}", Range)
                .BuildError<LangtType>();
        }

        foreach(var (ty, range) in bparams)
        {
            if(ty.IsReference)
            {
                builder.AddDgnError($"Cannot supply a reference type to a generic type", range);
            }

            if(ty == LangtType.None)
            {
                builder.AddDgnError($"Cannot supply 'none' to a generic type", range);
            }
        }
        
        if(!builder) return builder.BuildError<LangtType>();

        return builder.Build<LangtType>(new LangtConsGenericStructureType(baseTy.Structure, bparamTys));
    }
}
