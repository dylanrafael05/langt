

using Langt.Message;

namespace Langt.Structure;

public class GenericTypeSymbol : Symbol<LangtType>
{
    public required ISymbol<LangtType> Base {get; init;}
    public required ISymbol<LangtType>[] Arguments {get; init;}

    public override Result<LangtType> Unravel(Context ctx)
    {
        var builder = ResultBuilder.Empty();

        var baseTyRes = Base.Unravel(ctx);
        builder.AddData(baseTyRes);

        if(!baseTyRes) return builder.Build(LangtType.Error);

        var baseTy = baseTyRes.Value;

        if(!baseTy.IsStructure || !baseTy.IsConstructed)
        {
            return builder
                .WithDgnError(Messages.Get("expected-generic", baseTy), Range)
                .Build(LangtType.Error);
        }

        var args = new List<LangtType>();
        foreach(var arg in Arguments)
        {
            var argRes = arg.Unravel(ctx);
            builder.AddData(argRes);

            args.Add(argRes.Or(LangtType.Error));
        }
        
        if(Arguments.Length != baseTy.GenericParameters.Count)
        {
            return builder
                .WithDgnError(Messages.Get("generic-bad-arg-count", baseTy, args.Stringify(k => k.FullName), baseTy.GenericParameters.Count, Arguments.Length), Range)
                .BuildError<LangtType>();
        }
        
        foreach(var (idx, ty) in args.Indexed())
        {
            if(ty.IsReference)
            {
                builder.AddDgnError(Messages.Get("generic-ref-arg"), Arguments[idx].Range);
            }
        }

        if(!builder) return builder.BuildError<LangtType>();

        return builder.Build<LangtType>(new LangtConsGenericStructureType(baseTy.Structure, args.ToArray()));
    }
}

public class LangtConsGenericStructureType : LangtType, IStructureType
{
    public LangtConsGenericStructureType(IStructureType baseTy, LangtType[] args)
    {
        baseType = baseTy;
        constructionArguments = args;

        foreach(var a in args)
        {
            Expect.That(a.IsConstructed, "Constructed generic types cannot have unconstructed types");
        }
    }

    private readonly IStructureType baseType;
    private readonly LangtType[] constructionArguments;

    public IStructureType BaseType => baseType;
    public IReadOnlyList<LangtType> ConstructionArguments => constructionArguments;

    public override string Name        => baseType.Name        + $"!<{constructionArguments.Stringify(k => k.Name)}>";
    public override string DisplayName => baseType.DisplayName + $"!<{constructionArguments.Stringify(k => k.DisplayName)}>";
    public override string FullName    => baseType.FullName    + $"!<{constructionArguments.Stringify(k => k.FullName)}>";

    public IScope? TypeScope => baseType.TypeScope;

    public IEnumerable<string> FieldNames => baseType.FieldNames;

    public override bool Equals(LangtType? other)
        => other is LangtConsGenericStructureType o
        && baseType == o.baseType
        && constructionArguments.SequenceEqual(o.constructionArguments);

    public bool ResolveField(string name, out LangtStructureField field)
    {
        var ret = baseType.ResolveField(name, out var newfield);

        if(ret) 
        {
            var f = newfield;

            newfield = f with 
            {
                Type = f.Type.ReplaceAllGenerics(baseType.GenericParameters, constructionArguments)
            };
        }

        field = newfield;

        return ret;
    }

    public override LangtType ReplaceGeneric(LangtType ty, LangtType newty)
    {
        var cons = constructionArguments.Select(k => k.ReplaceGeneric(ty, newty));

        return new LangtConsGenericStructureType(baseType, cons.ToArray());
    }

    public bool HasField(string name)
    {
        return baseType.HasField(name);
    }

    public override bool? TestAgainstFloating(Func<LangtType, bool?> pred)
        => pred(this) ?? this.Fields().FloatingAny(f => f.Type.TestAgainstFloating(pred));
}