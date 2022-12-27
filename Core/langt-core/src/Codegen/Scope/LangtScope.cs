using System.Diagnostics.CodeAnalysis;
using Langt.AST;
using Langt.Codegen;

namespace Langt.Codegen;

public class LangtScope : IScoped
{
    public LangtScope? HoldingScope {get; set;}

    /// <summary>
    /// Get the nearest namespace to the current scope
    /// </summary>
    public LangtNamespace? NearestNamespace 
        => HoldingScope is LangtNamespace ns ? ns : HoldingScope?.NearestNamespace;

    public virtual bool IsNamespace => false;
    public bool IsGlobalScope => HoldingScope == null;

    private readonly HashSet<string> definedNames = new();
    private readonly List<IScoped> unnamedItems = new();
    private readonly Dictionary<string, INamedScoped> namedItems = new();

    public IReadOnlySet<string> DefinedNames => definedNames;
    public IReadOnlyList<IScoped> UnnamedItems => unnamedItems;
    public IReadOnlyDictionary<string, INamedScoped> NamedItems => namedItems;
    public IEnumerable<IScoped> AllItems => namedItems.Values.Concat(unnamedItems);

    public void AddUnnamed(IScoped item) 
    {
        unnamedItems.Add(item);
        item.HoldingScope = this;
    }
    public LangtScope AddUnnamedScope() 
    {
        var scope = new LangtScope();
        AddUnnamed(scope);
        return scope;
    }

    // TODO: replace 'null' returns with Result.Error
    public Result<INamedScoped> Resolve(string input,
                                 SourceRange range,
                                 bool propogate = true)
        => Resolve<INamedScoped>(input, "item", range, propogate);

    public virtual Result<TOut> Resolve<TOut>(string input,
                                              string outputType,
                                              SourceRange range,
                                              bool propogate = true) where TOut: class, INamedScoped
    {
        var builder = ResultBuilder.Empty();

        // Check if the item exists in the named items stored by this scope
        if(namedItems.TryGetValue(input, out var r))
        {
            // If the item is found and is of the expected type, return it
            if(r is TOut t) return builder.Build(t);

            // If the above condition failed,
            // produce a warning that an ambiguity was present but not fatal
            builder.AddWarning($"Possible reference candidate {r.FullName} found, but expected a {outputType}; try to disambiguate", range);
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

        // Return the result, null or not
        return result is null ? builder.Build<TOut>() : builder.Build(result.Value.Value);
    }

    public Result<LangtVariable> ResolveVariable(string name, SourceRange range, bool propogate = true) 
        => Resolve<LangtVariable>(name, "variable", range, propogate: propogate);
    public Result<LangtType> ResolveType(string name, SourceRange range, bool propogate = true) 
        => Resolve<LangtType>(name, "type", range, propogate: propogate);
    public Result<LangtFunctionGroup> ResolveFunctionGroup(string name, SourceRange range, bool propogate = true) 
        => Resolve<LangtFunctionGroup>(name, "function", range, propogate: propogate);
    public Result<LangtNamespace> ResolveNamespace(string name, SourceRange range, bool propogate = true) 
        => Resolve<LangtNamespace>(name, "namespace", range, propogate: propogate);

    public virtual Result Define(
        INamedScoped obj,
        SourceRange sourceRange)
    {
        if(definedNames.Contains(obj.Name))
        {
            return ResultBuilder.Empty().WithDgnError($"Attempting to redefine name {obj.Name}", sourceRange).Build();
        }

        namedItems.Add(obj.Name, obj);
        definedNames.Add(obj.Name);

        obj.HoldingScope = this;
        
        return Result.Success();
    }

    public Result DefineVariable(LangtVariable variable, SourceRange range)
        => Define(variable, range);
    public Result DefineType(LangtType type, SourceRange range)
        => Define(type, range);
    public Result DefineFunctionGroup(LangtFunctionGroup function, SourceRange range)
        => Define(function, range);
    public Result DefineNamespace(LangtNamespace ns, SourceRange range)
        => Define(ns, range);
}