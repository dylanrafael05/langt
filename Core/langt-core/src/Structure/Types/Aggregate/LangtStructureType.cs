using System.Collections;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class StructureTypeBuilder : IEnumerable<LangtStructureField>
{
    public StructureTypeBuilder(IScope tyScope) 
    {
        fields = new();
        this.tyScope = tyScope;
    }

    private readonly Dictionary<string, LangtStructureField> fields;
    private readonly IScope tyScope;

    private bool isNamed;
    private string? name;
    private IScope? holding;
    private SourceRange defRange;
    private LangtType[]? genTypes;

    public StructureTypeBuilder Named(string name) 
    {
        isNamed = true;
        this.name = name;

        return this;
    }
    public StructureTypeBuilder InScope(IScope? holding)
    {
        isNamed = true;
        this.holding = holding;

        return this;
    }
    public StructureTypeBuilder DefinedAt(SourceRange defRange)
    {
        isNamed = true;
        this.defRange = defRange;

        return this;
    }
    public StructureTypeBuilder WithGenericTypes(params LangtType[] genTypes)
    {
        isNamed = true;
        this.genTypes = genTypes;

        return this;
    }


    /// <summary>
    /// An alias of AddField.
    /// </summary>
    public void Add(string name, Weak<LangtType> ty) 
        => WithField(name, ty);
    public void AddField(string name, Weak<LangtType> ty) 
        => WithField(name, ty);
    public StructureTypeBuilder WithField(string name, Weak<LangtType> ty)
    {
        fields.Add(name, new(name, ty, fields.Count));
        return this;
    }

    public IStructureType Build()
    {
        if(!isNamed) return new LangtStructureType(tyScope, fields);

        Expect.NonNull(name);
        Expect.NonNull(holding);
        Expect.NonNull(defRange);
        
        genTypes ??= Array.Empty<LangtType>();

        return new LangtNamedStructureType(name!, fields, holding!, tyScope, genTypes)
        {
            DefinitionRange = defRange
        };
    }


    public IEnumerator<LangtStructureField> GetEnumerator()
        => fields.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => fields.Values.GetEnumerator();
}

public class LangtStructureType : LangtType, IStructureType
{
    public LangtStructureType(IScope typeScope, IReadOnlyDictionary<string, LangtStructureField> fields)
    {
        TypeScope = typeScope;
        fieldDict = fields;
    }

    public IScope TypeScope {get;}

    public IEnumerable<string> FieldNames => fieldDict.Keys;
    private readonly IReadOnlyDictionary<string, LangtStructureField> fieldDict;

    public virtual bool ResolveField(string name, out LangtStructureField field) 
    {
        if(!fieldDict.ContainsKey(name))
        {
            field = default;
            return false;
        }

        field = fieldDict[name];
        return true;
    }
    public bool HasField(string name) => fieldDict.ContainsKey(name);

    public override bool Contains(LangtType ty)
        => this.Fields().Any(k => k.Type.Expect().Contains(ty)) || base.Contains(ty);

    public override bool Equals(LangtType? other)
        => other is not null
        && other.IsStructure
        && this.Fields().SequenceEqual(other.Structure.Fields());
}

