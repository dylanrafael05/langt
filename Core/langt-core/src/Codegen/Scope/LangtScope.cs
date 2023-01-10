using System.Diagnostics.CodeAnalysis;
using Langt.AST;
using Langt.Codegen;

namespace Langt.Codegen;

public class LangtScope
{
    public LangtScope(LangtScope? holdingScope)
    {
        HoldingScope = holdingScope;
    }
    
    public LangtScope? HoldingScope {get;}

    /// <summary>
    /// Get the nearest namespace to the current scope
    /// </summary>
    public LangtNamespace? NearestNamespace 
        => HoldingScope is LangtNamespace ns ? ns : HoldingScope?.NearestNamespace;

    public virtual bool IsNamespace => false;
    public bool IsGlobalScope => HoldingScope == null;

    private readonly Dictionary<string, Resolution> namedItems = new();
    public IReadOnlyDictionary<string, Resolution> NamedItems => namedItems;

    public LangtScope CreateUnnamedSubScope() 
    {
        var scope = new LangtScope(this);
        return scope;
    }

    // TODO: replace 'null' returns with Result.Error
    public Result<Resolution> Resolve(string input,
                                 SourceRange range,
                                 bool propogate = true)
        => Resolve<Resolution>(input, "item", range, propogate);

    public virtual Result<TOut> Resolve<TOut>(string input,
                                              string outputType,
                                              SourceRange range,
                                              bool propogate = true) where TOut: Resolution
    {
        var builder = ResultBuilder.Empty();

        // Check if the item exists in the named items stored by this scope
        if(namedItems.TryGetValue(input, out var r) && r is TOut t)
        {
            return builder.Build(t);
        }

        Result<TOut>? result = null;

        // Attempt to propogate values if permitted
        if(propogate)
        {
            // Check the upper scope if it exists
            result = HoldingScope?.Resolve<TOut>(input, outputType, range);
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
        return result is null || builder.HasErrors ? builder.Build<TOut>() : builder.Build(result.Value.Value);
    }

    public Result<LangtVariable> ResolveVariable(string name, SourceRange range, bool propogate = true) 
        => Resolve<LangtVariable>(name, "variable", range, propogate: propogate);
    public Result<LangtType> ResolveType(string name, SourceRange range, bool propogate = true) 
        => Resolve<LangtType>(name, "type", range, propogate: propogate);
    public Result<LangtFunctionGroup> ResolveFunctionGroup(string name, SourceRange range, bool propogate = true) 
        => Resolve<LangtFunctionGroup>(name, "function", range, propogate: propogate);
    public Result<LangtNamespace> ResolveNamespace(string name, SourceRange range, bool propogate = true) 
        => Resolve<LangtNamespace>(name, "namespace", range, propogate: propogate);

    public virtual Result<T> Define<T>(Func<LangtScope, T> constructor, SourceRange sourceRange) where T : Resolution
    {
        var obj = constructor(this);

        if(namedItems.ContainsKey(obj.Name))
        {
            return ResultBuilder
                .Empty()
                .WithDgnError($"Attempting to redefine name {obj.Name}", sourceRange)
                .Build<T>();
        }

        namedItems.Add(obj.Name, obj);
        
        return Result.Success(obj);
    }
}