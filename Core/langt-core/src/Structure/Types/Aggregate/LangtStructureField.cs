namespace Langt.Structure;

public readonly struct LangtStructureField : IEquatable<LangtStructureField>
{
    public LangtStructureField(string name, LangtType type, int index) 
    {
        Name = name;
        Type = type;
        Index = index;

        Expect.That(Type.IsConstructed, "Structure types can only contain constructed types");
    }

    public string Name {get; init;} 
    public LangtType Type {get; init;}
    public int Index {get; init;}

    public bool Equals(LangtStructureField other)
        => Name == other.Name 
        && Type == other.Type;

    public override bool Equals(object? obj)
        => obj is LangtStructureField k && Equals(k);

    public override int GetHashCode()
        => HashCode.Combine(Name, Type);

    public static bool operator ==(LangtStructureField? a, LangtStructureField? b)
        => a is null ? b is null : a.Equals(b);
    public static bool operator !=(LangtStructureField? a, LangtStructureField? b) 
        => !(a == b);
}
