namespace Langt.Structure;
using Resolutions;

public static class ScopeExtensions
{
    public static string FullNameDefault(this INamed item) 
    {
        var name = item is IFullNamed fn ? fn.DisplayName : item.Name;

        if(item is not IResolution resolution)  
            return name;
        
        var holding = resolution.HoldingScope;

        while(holding is not INamed && holding is not null)     
            holding = holding.HoldingScope;

        if(holding is null)
            return name;

        var upperName = FullNameDefault((INamed)holding);

        return upperName + "::" + name;
    }

    public static Result DefineProxy<T>(this IScope scope, T item, SourceRange range) where T : IFullNamed
    {
        return scope.Define<IResolutionProducer<T>>(s => new ResolutionProxy<T>(item, s), range).Drop();
    }

    public static Result Define<TIn>(this IScope scope, Func<IScope, TIn> constructor, SourceRange sourceRange, out TIn? result) where TIn : IResolution
    {
        var s = scope.Define(constructor, sourceRange);
        result = s.OrDefault();
        return s.Drop();
    }
    public static Result Define<TIn>(this IScope scope, Func<IScope, TIn> constructor, SourceRange sourceRange, Action<TIn?> handler) where TIn : IResolution
    {
        var s = scope.Define(constructor, sourceRange);
        handler(s.OrDefault());
        return s.Drop();
    }

    public static Result<TIn> Define<TIn>(this IScope scope, Func<IScope, TIn> constructor, SourceRange sourceRange, SourceRange nameRange, ResultBuilder builder) where TIn : IResolution
    {
        var s = scope.Define(constructor, sourceRange);

        if(s.HasValue)
        {
            builder.AddStaticReference(nameRange, s.Value, true);
        }

        return s;
    }
    public static Result Define<TIn>(this IScope scope, Func<IScope, TIn> constructor, SourceRange sourceRange, SourceRange nameRange, ResultBuilder builder, out TIn? result) where TIn : IResolution
    {
        var s = scope.Define(constructor, sourceRange, nameRange, builder);
        result = s.OrDefault();
        return s.Drop();
    }
    public static Result Define<TIn>(this IScope scope, Func<IScope, TIn> constructor, SourceRange sourceRange, SourceRange nameRange, ResultBuilder builder, Action<TIn?> handler) where TIn : IResolution
    {
        var s = scope.Define(constructor, sourceRange, nameRange, builder);
        handler(s.OrDefault());
        return s.Drop();
    }
    
    public static Result<WeakRes<LangtVariable>> ResolveVariable(this IScope sc, string name, SourceRange range) 
        => sc.Resolve<LangtVariable>(name, "variable", range);
    public static Result<WeakRes<LangtType>> ResolveType(this IScope sc, string name, SourceRange range) 
        => sc.Resolve<LangtType>(name, "type", range);
public static Result<WeakRes<LangtFunctionGroup>> ResolveFunctionGroup(this IScope sc, string name, SourceRange range) 
        => sc.Resolve<LangtFunctionGroup>(name, "function", range);
    public static Result<WeakRes<LangtNamespace>> ResolveNamespace(this IScope sc, string name, SourceRange range) 
        => sc.Resolve<LangtNamespace>(name, "namespace", range);

    public static Result<WeakRes<IResolution>> Resolve(this IScope sc, string input, SourceRange range)
        => sc.Resolve<IResolution>(input, "item", range);
}