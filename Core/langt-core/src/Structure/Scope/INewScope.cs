using System.Runtime.CompilerServices;
using Langt.AST;

namespace Langt.Structure;

// public readonly struct UniqueItemIdentifier : IEquatable<UniqueItemIdentifier>
// {
//     public override bool Equals(object? other)
//         => other is not null && Equals((UniqueItemIdentifier)other);
//     public override int GetHashCode()
//     {
//         var hash = new HashCode();

//         hash.Add(Type);

//         foreach(var k in Keys)
//         {
//             hash.Add(k);
//         }

//         return hash.ToHashCode();
//     }

//     public bool Equals(UniqueItemIdentifier other)
//     {
//         if(Type != other.Type) return false;
//         if(Keys.Length != other.Keys.Length) return false;

//         for(var i = 0; i < Keys.Length; i++)
//         {
//             if(!Keys[i].Equals(other.Keys[i])) return false;
//         }

//         return true;
//     }

//     public static bool operator ==(UniqueItemIdentifier a, UniqueItemIdentifier b)
//         => a.Equals(b);
//     public static bool operator !=(UniqueItemIdentifier a, UniqueItemIdentifier b) 
//         => !(a == b);


//     public Type Type {get; init;} 
//     public object[] Keys {get; init;}

//     public UniqueItemIdentifier(Type type, params object[] keys)
//     {
//         Type = type;
//         Keys = keys;
//     }
// }

public enum CompletionState : byte
{
    Incomplete,
    InProgress,
    Complete
}

public interface IResolutionlike : IFullNamed
{
    IScope HoldingScope {get;}
    string? Documentation {get;}
}

public interface IResolvable : IResolutionlike
{
    CompletionState Completion {get;}
    Result Complete(Context ctx);

    static virtual string TypeName => "item";
}

public class ProxyResolution : ImmediateResolvable
{
    [SetsRequiredMembers]
    public ProxyResolution(IScope holdingScope, IFullNamed item)
    {
        Item = item;

        Name = item.Name;
        HoldingScope = holdingScope;
    }
    public IFullNamed Item {get;}
}


public interface ISymbol : ISourceRanged
{
    Result<object> Unravel(Context ctx);
}

public interface ISymbol<T> : ISymbol
{
    Result<object> ISymbol.Unravel(Context ctx)
        => Unravel(ctx).As<object>();
    new Result<T> Unravel(Context ctx);
}

public static class SymbolExtensions
{
    private readonly struct CastSymbol<T> : ISymbol<T>
    {
        public ISymbol Source {get; init;}
        public string TypeName {get; init;}

        public SourceRange Range => Source.Range;
        public Result<T> Unravel(Context ctx)
        {
            var k = Source.Unravel(ctx);
            if(!k) return k.ErrorCast<T>();

            if(k.Value is not T)
            {
                return k
                    .WithError(Diagnostic.Error($"Expected to find a {TypeName} but did not", Range))
                    .ErrorCast<T>();
            }

            return k.As<T>();
        }
    }

    public static ISymbol<T> As<T>(this ISymbol symbol, string typename)
    {
        return new CastSymbol<T>
        {
            Source = symbol,
            TypeName = typename
        };
    }

    public static ISymbol<T> As<T>(this ISymbol symbol) where T : IResolvable
        => symbol.As<T>(T.TypeName);
}

public abstract class Symbol<T> : ISymbol<T>
{
    public virtual SourceRange Range { get; init; }
    public abstract Result<T> Unravel(Context ctx);
}

public class DirectResolutionSymbol : ISymbol<IResolvable>
{
    public DirectResolutionSymbol(SourceRange range, IScope scope, string name) 
    {
        Range = range;
        Name = name;
        HoldingScope = scope;
    }

    public SourceRange Range {get; init;}
    public string Name {get; init;}
    public IScope HoldingScope {get; init;}

    public Result<IResolvable> Unravel(Context ctx) 
    {
        return HoldingScope.Resolve(Name, Range, ctx);
    }
}

public class ResolutionSymbol : ISymbol<IResolvable>
{
    public ResolutionSymbol(SourceRange range, ISymbol<IScope> scope, string name) 
    {
        Range = range;
        Name = name;
        SearchScope = scope;
    }

    public SourceRange Range {get; init;}
    public string Name {get; init;}
    public ISymbol<IScope> SearchScope {get; init;}

    public Result<IResolvable> Unravel(Context ctx) 
    {
        var sres = SearchScope.Unravel(ctx);
        if(!sres) return sres.As<IResolvable>();

        return sres.Value.Resolve(Name, Range, ctx, true);
    }
}

public abstract class Resolvable : IResolvable
{
    public virtual CompletionState Completion {get; protected set;}

    public required string Name {get; init;}
    public required IScope HoldingScope {get; init;}

    public string? Documentation {get; init;}

    public string DisplayName => Name;
    public string FullName => this.FullNameSimple();

    Result IResolvable.Complete(Context ctx)
    {
        Completion = CompletionState.InProgress;
        var r = Complete(ctx);
        Completion = CompletionState.Complete;

        return r;
    }

    public abstract Result Complete(Context ctx);
}

public abstract class ImmediateResolvable : Resolvable
{
    public ImmediateResolvable()
    {
        Completion = CompletionState.Complete;
    }

    public sealed override Result Complete(Context ctx)
        => Result.Success();
}

public static class ResolvableExtensions
{
    public static bool ResolutionEqual(this IResolutionlike self, IResolutionlike other)
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
    
    public static bool Incomplete(this CompletionState state) 
        => state < CompletionState.Complete;

    public static void AssertIncomplete(this IResolvable self, [CallerMemberName] string callerName = "!")
    {
        Expect.That(self.Completion.Incomplete(), $"Resolution must be incomplete to call .{callerName}");
    }
    
    public static IFullNamed UnwrapProxy(this IResolvable self) => self switch 
    {
        ProxyResolution proxy => proxy.Item,
        _                     => self
    };

    public static Result<IFullNamed> UnwrapProxy(this Result<IResolvable> self)
        => self.Map(UnwrapProxy);
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

public interface ISymbolProvider<T>
{
    public ISymbol<T> GetSymbol(Context ctx);
}

public static class SymbolProviderExtensions
{
    public static Result<T> UnravelSymbol<T>(this ISymbolProvider<T> self, Context ctx)
        => self.GetSymbol(ctx).Unravel(ctx);
}

public interface IScope
{
    IScope? Parent {get;}
    Result<IResolvable> Resolve(string identifier, SourceRange range, Context ctx, bool upwards = true);
    Result Define(IResolvable value, SourceRange range); 
}

public static class ScopeExtensions
{
    public static ISymbol<IResolvable> ResolveSymbol<T>(this ISymbol<T> sc, string name, SourceRange range) where T : IScope
        => new ResolutionSymbol(range, sc.As<IScope>("scope"), name);
    public static ISymbol<IResolvable> ResolveSymbol(this IScope sc, string name, SourceRange range)
        => new DirectResolutionSymbol(range, sc, name);

    public static Result DefineProxy(this IScope sc, IFullNamed item, SourceRange range)
        => sc.Define(new ProxyResolution(sc, item), range);
}

public class SimpleScope : IScope
{
    private readonly Dictionary<string, IResolvable> items = new();

    public required IScope? Parent {get; init;}
    
    public Result<IResolvable> Resolve(string identifier, SourceRange range, Context ctx, bool upwards = true)
    {
        if (items.TryGetValue(identifier, out var resultant))
        {
            if(resultant.Completion == CompletionState.Incomplete)
            {
                var res = resultant.Complete(ctx);

                if(!res) return Result.Blank<IResolvable>().WithDataFrom(res);
            }

            return Result.Success(resultant);
        }
        else if (upwards && Parent is not null)
        {
            return Parent.Resolve(identifier, range, ctx, true);
        }
        else 
        {
            return Result.Error<IResolvable>(Diagnostic.Error($"No item named {identifier} found in scope", range));
        }
    }

    public Result Define(IResolvable value, SourceRange range)
    {
        if (items.ContainsKey(value.Name))
        {
            return Result.Error(Diagnostic.Error($"Attempt to redefine {value.FullName}", range));
        }

        items.Add(value.Name, value);

        return Result.Success();
    }
}

public class Namespace : ImmediateResolvable, IScope
{
    [SetsRequiredMembers]
    public Namespace(IScope parent, string name)
    {
        scope = new SimpleScope {Parent = parent};
        HoldingScope = parent;
        Name = name;
    }

    private readonly SimpleScope scope;

    IScope IScope.Parent => HoldingScope;


    public Result Define(IResolvable value, SourceRange range)
        => scope.Define(value, range);
    public Result<IResolvable> Resolve(string identifier, SourceRange range, Context ctx, bool upwards = true)
        => scope.Resolve(identifier, range, ctx, upwards);

    public static string TypeName => "namespace";
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

    public Result Define(IResolvable value, SourceRange range)
        => curScope.Define(value, range);

    public Result<IResolvable> Resolve(string identifier, SourceRange range, Context ctx, bool upwards = true)
    {
        var r = curScope.Resolve(identifier, range, ctx, upwards);

        if(!r) 
        {
            foreach(var n in namespaces)
            {
                r = n.Resolve(identifier, range, ctx, upwards);
                if(r) break;
            }
        }

        return r;
    }
}


/*

    New compiler steps:

        *- Symbol Creation
        *- Symbol Resolution
        !- Type Checking
        !- Binding

    Symbols are "partially delayed":
    A symbol which represents a resolution stores the *location* of the resolution
    rather than the *value* of the resolution. For instance...

        ?class Variable(string Name, LangtType type);
        ?class VariableSymbol(string Name, TypeSymbol type);

        ?class TypeSymbol(string Name, IScope Holding) : ISymbol<LangtType>;
    
    The following interfaces would need to be created:

        *- ISymbol<T> : Result<T> Actualize();
        *- IType : basic attributes of any type 

    How to handle recursive symbols?

        struct Test 
        {
            ptr *Test //valid
        }

        struct Err
        {
            val Err //error!
        }
        
        recursive types will need to be mutable. This can be mitigated through some form of
        IComplexInitializable interface which allows for modification when some boolean 'IsInitializing' 
        flag is set to true.

        TODO: improve Result interface for future use
        TODO: add mutable "interface" to recursive items

*/