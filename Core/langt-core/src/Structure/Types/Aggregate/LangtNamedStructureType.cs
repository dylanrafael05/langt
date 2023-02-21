using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtNamedStructureType : LangtResolvableType, IStructureType
{
    public LangtNamedStructureType(string name, IReadOnlyDictionary<string, LangtStructureField> fields, IScope scope, IScope typeScope, IReadOnlyList<LangtType> genericParameters) : base(name, scope)
    {
        inner = new(typeScope, fields);
        GenericParameters = genericParameters;

        TypeScope = typeScope;
    }

    private readonly LangtStructureType inner;
    
    public override IReadOnlyList<LangtType> GenericParameters {get;}
    public override bool IsConstructed => GenericParameters.Count == 0;

    [NotNull] public IScope? TypeScope {get;}

    public override bool IsBuiltin => true;

    public IEnumerable<string> FieldNames => ((IStructureType)inner).FieldNames;

    public override bool Contains(LangtType ty)
        => inner.Contains(ty);

    public bool ResolveField(string name, out LangtStructureField field)
        => inner.ResolveField(name, out field);

    public bool HasField(string name)
        => inner.HasField(name);
}
