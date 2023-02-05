using Langt.Structure.Resolutions;

namespace Langt.Structure;

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

    public void AddField(string name, LangtType ty)
    {
        baseType.AddField(name, ty);
    }
}