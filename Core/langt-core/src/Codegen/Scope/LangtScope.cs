using System.Diagnostics.CodeAnalysis;
using Langt.AST;
using Langt.Codegen;

namespace Langt.Codegen;

public class LangtScope : IScope
{
    public LangtScope(IScope? holdingScope)
    {
        HoldingScope = holdingScope;
    }
    
    public IScope? HoldingScope {get;}

    public virtual bool IsNamespace => false;
    public bool IsGlobalScope => HoldingScope == null;

    private readonly Dictionary<string, IResolution> namedItems = new();
    public IReadOnlyDictionary<string, IResolution> NamedItems => namedItems;

    public virtual Result<TOut> Resolve<TOut>(string input,
                                              string outputType,
                                              SourceRange range,
                                              bool propogate = true) where TOut: INamed
    {
        var builder = ResultBuilder.Empty();

        // Check if the item exists in the named items stored by this scope
        if(namedItems.TryGetValue(input, out var r))
        {
            if(r is TOut t)                       return builder.Build(t);
            if(r is IProxyResolution<TOut> proxy) return builder.Build(proxy.Inner);
        }

        Result<TOut>? result = null;

        // Attempt to propogate values if permitted
        if(propogate)
        {
            // Check the upper scope if it exists
            result = HoldingScope?.Resolve<TOut>(input, outputType, range, propogate);
        }
        
        if(result is null)
        {
            builder.AddDgnError($"Could not find {outputType} named {input}", range);
        }
        else
        {
            builder.AddData(result.Value);
        }

        // Return the result, null or not
        return result is null || builder.HasErrors ? builder.BuildError<TOut>() : builder.Build(result.Value.Value);
    }

    public virtual Result<T> Define<T>(Func<LangtScope, T> constructor, SourceRange sourceRange) where T : IResolution
    {
        var obj = constructor(this);

        if(namedItems.ContainsKey(obj.Name))
        {
            return ResultBuilder
                .Empty()
                .WithDgnError($"Attempting to redefine name {obj.Name}", sourceRange)
                .BuildError<T>();
        }

        namedItems.Add(obj.Name, obj);
        
        return Result.Success(obj);
    }
}