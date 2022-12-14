using System.Diagnostics.CodeAnalysis;
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

    public INamedScoped? Resolve(string input,
                                 SourceRange range,
                                 DiagnosticCollection diagnostics,
                                 bool err = true,
                                 bool entry = true,
                                 bool propogate = true)
        => Resolve<INamedScoped>(input, "item", range, diagnostics, err, entry, propogate);

    public virtual TOut? Resolve<TOut>(string input,
                                       string outputType,
                                       SourceRange range,
                                       DiagnosticCollection diagnostics,
                                       bool err = true,
                                       bool entry = true,
                                       bool propogate = true) where TOut: class, INamedScoped
    {
        // Check if the item exists in the named items stored by this scope
        if(namedItems.TryGetValue(input, out var r))
        {
            // If the item is found and is of the expected type, return it
            if(r is TOut t) return t;

            // If the above condition failed,
            // produce a warning that an ambiguity was present but not fatal
            diagnostics.Warning($"Possible reference candidate {r.FullName} found, but expected a {outputType}; try to disambiguate", range);
        }

        TOut? result = null;

        // Attempt to propogate values if permitted
        if(propogate)
        {
            // Check the upper scope if it exists
            result = HoldingScope?.Resolve<TOut>(input, outputType, range, diagnostics, err, false);

            // If allowed, produce a resolution error
            if(entry && err && result is null)
            {
                diagnostics.Error($"Could not find {outputType} named {input}", range);
            }
        }

        // Return the result, null or not
        return result;
    }

    public LangtVariable? ResolveVariable(string name, SourceRange range, DiagnosticCollection diagnostics, bool err = true, bool propogate = true) 
        => Resolve<LangtVariable>(name, "variable", range, diagnostics, err, propogate: propogate);
    public LangtType? ResolveType(string name, SourceRange range, DiagnosticCollection diagnostics, bool err = true, bool propogate = true) 
        => Resolve<LangtType>(name, "type", range, diagnostics, err, propogate: propogate);
    public LangtFunctionGroup? ResolveFunctionGroup(string name, SourceRange range, DiagnosticCollection diagnostics, bool err = true, bool propogate = true) 
        => Resolve<LangtFunctionGroup>(name, "function", range, diagnostics, err, propogate: propogate);
    public LangtNamespace? ResolveNamespace(string name, SourceRange range, DiagnosticCollection diagnostics, bool err = true, bool propogate = true) 
        => Resolve<LangtNamespace>(name, "namespace", range, diagnostics, err, propogate: propogate);

    public virtual bool Define(
        INamedScoped obj,
        SourceRange sourceRange, 
        DiagnosticCollection collector)
    {
        if(definedNames.Contains(obj.Name))
        {
            collector.Error($"Attempting to redefine name {obj.Name}", sourceRange);
            return false;
        }

        namedItems.Add(obj.Name, obj);
        definedNames.Add(obj.Name);

        obj.HoldingScope = this;
        
        return true;
    }

    public virtual void ForceDefine(INamedScoped obj)
    {
        if(definedNames.Contains(obj.Name))
        {
            throw new Exception();
        }

        namedItems.Add(obj.Name, obj);
        definedNames.Add(obj.Name);
        
        obj.HoldingScope = this;
    }

    public bool DefineVariable(LangtVariable variable, SourceRange range, DiagnosticCollection collector)
        => Define(variable, range, collector);
    public bool DefineType(LangtType type, SourceRange range, DiagnosticCollection collector)
        => Define(type, range, collector);
    public bool DefineFunctionGroup(LangtFunctionGroup function, SourceRange range, DiagnosticCollection collector)
        => Define(function, range, collector);
    public bool DefineNamespace(LangtNamespace ns, SourceRange range, DiagnosticCollection collector)
        => Define(ns, range, collector);
    
    public void ForceDefineVariable(LangtVariable variable)
        => ForceDefine(variable);
    public void ForceDefineType(LangtType type)
        => ForceDefine(type);
    public void ForceDefineFunctionGroup(LangtFunctionGroup function)
        => ForceDefine(function);
    public void ForceDefineNamespace(LangtNamespace ns)
        => ForceDefine(ns);
}