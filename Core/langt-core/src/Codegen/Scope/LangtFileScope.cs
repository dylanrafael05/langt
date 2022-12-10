namespace Langt.Codegen;

public class LangtFileScope : LangtScope
{
    public LangtFileScope(LangtScope parent)
    {
        HoldingScope = parent;
    }

    // A list of namespaces included by the source code with 'using blah.blah.blah' directives
    public List<LangtNamespace> IncludedNamespaces {get; init;} = new();

    public override TOut? Resolve<TOut>(string input, string outputType, SourceRange range, DiagnosticCollection diagnostics, bool err = true, bool entry = true, bool propogate = true) where TOut : class
    {
        // Get basic result, allowing errors if propogation is absent
        var baseResult = HoldingScope?.Resolve<TOut>(input, outputType, range, diagnostics, err && !propogate, false, propogate);

        // End propogation here if necessary
        if(!propogate) return baseResult;

        // Accumulate all non-null results into this list
        var allResults = new HashSet<TOut>();
        if(baseResult is not null) allResults.Add(baseResult);

        foreach(var n in IncludedNamespaces)
        {
            // Include the given namespaces in the search, 
            // but do not produce for resolution errors (note the 'false')
            var nResult = n.Resolve<TOut>(input, outputType, range, diagnostics, false, false, propogate);
            if(nResult is not null && !allResults.Contains(nResult)) allResults.Add(nResult);
        }

        // Return normal circumstances
        if(allResults.Count == 0) 
        {
            if(entry && err) 
            {
                diagnostics.Error($"Could not find {outputType} named {input}", range);
            }
            return null;
        }

        if(allResults.Count == 1) return allResults.First();

        // If allowed, produce an ambiguous resolution error
        if(entry && err)
        {
            diagnostics.Error(
                "Ambiguity between " + 
                string.Join(", ", allResults.Select(t => t.FullName)) +
                "; either disambiguate, remove includes, or use explicit '.' accesses"
            , range);
        }

        // Return null to signify a failed resolution
        return null;
    }

    public override bool Define(INamedScoped obj, SourceRange sourceRange, DiagnosticCollection collector)
        => HoldingScope!.Define(obj, sourceRange, collector);

    public override void ForceDefine(INamedScoped obj)
        => HoldingScope!.ForceDefine(obj);
}
