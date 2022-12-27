using Langt.AST;

namespace Langt.Codegen;

public class LangtFileScope : LangtScope
{
    public LangtFileScope(LangtScope parent)
    {
        HoldingScope = parent;
    }

    // A list of namespaces included by the source code with 'using blah.blah.blah' directives
    public List<LangtNamespace> IncludedNamespaces {get; init;} = new();

    public override Result<TOut> Resolve<TOut>(string input, string outputType, SourceRange range, bool propogate = true) where TOut : class
    {
        // Get basic result, allowing errors if propogation is absent
        var baseResult = HoldingScope!.Resolve<TOut>(input, outputType, range, propogate);

        // End propogation here if necessary
        if(propogate && !baseResult) return baseResult;

        // Accumulate all non-null results into this list
        var includedResults = Result.SkipForeach(IncludedNamespaces, n => n.Resolve<TOut>(input, outputType, range, propogate));

        var allResults = includedResults
            .Value
            .Append(baseResult.Value)
            .ToArray();

        var builder = ResultBuilder
            .From(includedResults)
            .WithData(baseResult);

        // Return normal circumstances
        if(allResults.Length == 0) return builder.WithDgnError($"Could not find {outputType} named {input}", range).Build<TOut>();

        if(allResults.Length == 1) return builder.Build(allResults.First());

        // If allowed, produce an ambiguous resolution error
        return builder.WithDgnError(
            "Ambiguity between " + 
            string.Join(", ", allResults.Select(t => t.FullName)) +
            "; either disambiguate, remove includes, or use explicit '.' accesses"
        , range).Build<TOut>();
    }

    public override Result Define(INamedScoped obj, SourceRange sourceRange)
        => HoldingScope!.Define(obj, sourceRange);
}
