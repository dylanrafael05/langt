namespace Langt.Codegen;

public static class ScopeExtensions
{
    public static Result DefineProxy<T>(this IScope scope, T item, SourceRange range) where T : INamed
    {
        return scope.Define<IProxyResolution<T>>(s => new ProxyResolution<T>(item, s), range).Drop();
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
    
    public static Result<LangtVariable> ResolveVariable(this IScope sc, string name, SourceRange range, bool propogate = true) 
        => sc.Resolve<LangtVariable>(name, "variable", range, propogate: propogate);
    public static Result<LangtType> ResolveType(this IScope sc, string name, SourceRange range, bool propogate = true) 
        => sc.Resolve<LangtType>(name, "type", range, propogate: propogate);
    public static Result<LangtFunctionGroup> ResolveFunctionGroup(this IScope sc, string name, SourceRange range, bool propogate = true) 
        => sc.Resolve<LangtFunctionGroup>(name, "function", range, propogate: propogate);
    public static Result<LangtNamespace> ResolveNamespace(this IScope sc, string name, SourceRange range, bool propogate = true) 
        => sc.Resolve<LangtNamespace>(name, "namespace", range, propogate: propogate);

    public static Result<IResolution> Resolve(this IScope sc, string input, SourceRange range, bool propogate = true)
        => sc.Resolve<IResolution>(input, "item", range, propogate);
}