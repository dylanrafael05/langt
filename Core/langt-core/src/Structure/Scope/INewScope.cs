namespace Langt.Structure;

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

public interface IResolvable
{
    ResolutionIdentifier Identifier {get;}

    sealed string Name => Identifier.Name;
    sealed string DisplayName => Identifier.DisplayName;
    sealed string FullName => Identifier.FullName;
    sealed IScope HoldingScope => Identifier.HoldingScope;
    sealed string? Documentation => Identifier.Documentation;

    static virtual string TypeName => "item";
}

public static class ResolvableExtensions
{
    public static bool ResolutionEqual(this IResolvable self, IResolvable other)
        => self.Name == other.Name && ReferenceEquals(self.HoldingScope, other.HoldingScope);

    public static string FullNameSimple(IScope? holdingScope, string displayName)
    {
        if(holdingScope is null) return displayName;

        var u = holdingScope;

        while(u.Parent is not IResolvable and not null)
        {
            u = u.Parent;
        }

        if(u is IResolvable r) return r.FullName + "::" + displayName;

        return displayName;
    }
    public static string FullNameSimple(this IResolvable self) 
        => FullNameSimple(self.HoldingScope, self.DisplayName);
}

public interface IWeak 
{
    object Identifier {get;}
    bool IsInitialized {get;}
    Result<object> Unravel();
}

public class ResolutionIdentifier : IEquatable<ResolutionIdentifier>
{
    public ResolutionIdentifier(string name, IScope holdingScope)
    {
        this.Name = name;
        this.HoldingScope = holdingScope;
    }

    public string Name {get;}
    public IScope HoldingScope {get;}
    public string? Documentation {get;} = null;
    public virtual string DisplayName => Name;
    public virtual string FullName => ResolvableExtensions.FullNameSimple(HoldingScope, DisplayName);

    public bool Equals(ResolutionIdentifier? other) 
        => other is not null
        && Name == other.Name 
        && HoldingScope == other.HoldingScope
        ;

    public override bool Equals(object? obj)
        => obj is ResolutionIdentifier ri && Equals(ri);

    public override int GetHashCode()
        => HashCode.Combine(Name, HoldingScope, Documentation, DisplayName, FullName);

    public static bool operator ==(ResolutionIdentifier? a, ResolutionIdentifier? b)
        => a is null ? b is null : a.Equals(b);
    public static bool operator !=(ResolutionIdentifier? a, ResolutionIdentifier? b)
        => !(a == b);
}

public struct Weak<T> : IWeak, IEquatable<Weak<T>>
{
    public required object Identifier {get; init;}
    public required ResultFunc<IWeak, T> Constructor {get; init;}

    public bool IsInitialized {get; private set;}
    private Result<T> val;

    public Result<T> Unravel()
    {
        if(IsInitialized) return val;

        IsInitialized = true;
        return val = Constructor(this);
    }
    public T Expect() 
        => Unravel().Expect();

    public Weak<K> As<K>()
    {
        Utility.Expect.That(typeof(K).IsAssignableFrom(typeof(T)));

        var cons = Constructor;

        return new()
        {
            Identifier  = Identifier,
            Constructor = x => cons(x).As<K>()
        };
    }

    Result<object> IWeak.Unravel() 
        => Unravel().As<object>();

    public bool Equals(Weak<T> other)
        => Identifier == other.Identifier;
    public override bool Equals(object? obj)
        => obj is Weak<T> weak && Equals(weak);

    public override int GetHashCode()
        => Identifier.GetHashCode();

    public static bool operator ==(Weak<T> a, Weak<T> b) 
        => a.Equals(b);
    public static bool operator !=(Weak<T> a, Weak<T> b) 
        => !(a == b);
}

public struct WeakResolution : IWeak, IResolvable
{
    public WeakResolution(ResolutionIdentifier identifier, ResultFunc<WeakResolution, IResolvable> constructor)
    {
        this.Identifier = identifier;

        value = new Weak<IResolvable>()
        {
            Identifier = identifier,
            Constructor = x => constructor((WeakResolution)x)
        };
    }

    public ResolutionIdentifier Identifier {get;}

    private Weak<IResolvable> value;

    public bool IsInitialized => value.IsInitialized;

    object IWeak.Identifier => Identifier;
    Result<object> IWeak.Unravel()
        => ((IWeak)value).Unravel();

    public Result<IResolvable> Unravel()
        => value.Unravel();
    public Weak<K> As<K>() where K : IResolvable
        => value.As<K>();
    public K Expect<K>() where K : IResolvable
        => As<K>().Unravel().Expect();

    public static WeakResolution Wrapping(IResolvable r) 
        => new(r.Identifier, _ => Result.Success(r));
}

public static class Weak 
{
    public static Weak<T> Wrapping<T>(object identifier, T value)
        => new() 
        {
            Identifier  = identifier,
            Constructor = _ => Result.Success(value)
        };
}

public interface IScope
{
    IScope? Parent {get;}
    Result<WeakResolution> Resolve(string identifier, bool upwards, SourceRange range);
    Result Define(WeakResolution value, SourceRange range); 
}

public class SimpleScope : IScope
{
    private readonly Dictionary<string, WeakResolution> items = new();

    public required IScope? Parent {get; init;}
    
    public Result<WeakResolution> Resolve(string identifier, bool upwards, SourceRange range)
    {
        if (items.TryGetValue(identifier, out var resultant))
        {
            return Result.Success(resultant);
        }
        else if (upwards && Parent is not null)
        {
            return Parent.Resolve(identifier, upwards, range);
        }
        else 
        {
            return Result.Error<WeakResolution>(Diagnostic.Error($"No item named {identifier} found in scope", range));
        }
    }

    public Result Define(WeakResolution value, SourceRange range)
    {
        if (items.ContainsKey(value.Identifier.Name))
        {
            return Result.Error(Diagnostic.Error($"Attempt to redefine {value.Identifier.FullName}", range));
        }

        items.Add(value.Identifier.Name, value);

        return Result.Success();
    }
}

public class Namespace : IScope, IResolvable
{
    public Namespace(ResolutionIdentifier identifier)
    {
        scope = new SimpleScope {Parent = identifier.HoldingScope};

        this.Identifier = identifier;
    }

    private readonly SimpleScope scope;
    public ResolutionIdentifier Identifier {get;}

    public IScope Parent => scope.Parent!;


    public Result Define(WeakResolution value, SourceRange range)
        => scope.Define(value, range);
    public Result<WeakResolution> Resolve(string identifier, bool upwards, SourceRange range)
        => scope.Resolve(identifier, upwards, range);
}

public class FileScope : IScope
{
    public FileScope(LangtProject proj)
    {
        curScope = proj.GlobalScope;
    }

    public void SetDefScope(IScope scope)
        => curScope = scope;

    public void AddNamespace(Namespace ns) 
        => namespaces.Add(ns);

    private IScope curScope;
    private readonly List<Namespace> namespaces = new();

    public IScope? Parent => throw new NotImplementedException();

    public Result Define(WeakResolution value, SourceRange range)
        => curScope.Define(value, range);

    public Result<WeakResolution> Resolve(string identifier, bool upwards, SourceRange range)
    {
        var r = curScope.Resolve(identifier, false, range);

        if(!r) 
        {
            foreach(var n in namespaces)
            {
                r = n.Resolve(identifier, false, range);
                if(r) break;
            }
        }

        return r;
    }
}