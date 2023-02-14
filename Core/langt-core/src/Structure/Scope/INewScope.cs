namespace Langt.Structure.Scope;

public readonly struct UniqueItemIdentifier : IEquatable<UniqueItemIdentifier>
{
    public override bool Equals(object? other)
        => other is not null && Equals((UniqueItemIdentifier)other);
    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(Type);

        foreach(var k in Keys)
        {
            hash.Add(k);
        }

        return hash.ToHashCode();
    }

    public bool Equals(UniqueItemIdentifier other)
    {
        if(Type != other.Type) return false;
        if(Keys.Length != other.Keys.Length) return false;

        for(var i = 0; i < Keys.Length; i++)
        {
            if(!Keys[i].Equals(other.Keys[i])) return false;
        }

        return true;
    }

    public static bool operator ==(UniqueItemIdentifier a, UniqueItemIdentifier b)
        => a.Equals(b);
    public static bool operator !=(UniqueItemIdentifier a, UniqueItemIdentifier b) 
        => !(a == b);


    public Type Type {get; init;} 
    public object[] Keys {get; init;}

    public UniqueItemIdentifier(Type type, params object[] keys)
    {
        Type = type;
        Keys = keys;
    }
}


public interface IScope
{
    
}
