using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtStructureType : LangtType, IStructureType
{
    public LangtStructureType(IScope typeScope)
    {
        TypeScope = typeScope;
    }

    public IScope TypeScope {get;}

    public IEnumerable<string> FieldNames => fieldDict.Keys;
    private readonly Dictionary<string, LangtStructureField> fieldDict = new();
    

    public void AddField(string name, LangtType ty)
    {
        fieldDict.Add(name, new(name, ty, fieldDict.Count));
    }

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
        => this.Fields().Any(k => k.Type.Contains(ty)) || base.Contains(ty);

    public override bool Equals(LangtType? other)
        => other is not null
        && other.IsStructure
        && this.Fields().SequenceEqual(other.Structure.Fields());
}

