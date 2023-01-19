using Langt.AST;

namespace Langt.Codegen;

public class LangtFileScope : LangtScope
{
    public LangtFileScope(IScope scope) : base(scope)
    {
        Expect.ArgNonNull(scope, "File scopes must have an enclosing scope!");
    }

    // A list of namespaces included by the source code with 'using blah.blah.blah' directives
    public List<LangtNamespace> IncludedNamespaces {get; init;} = new();

    public override Result<TOut> Resolve<TOut>(string input, string outputType, SourceRange range, bool propogate = true)
    {
        // Get basic result, allowing errors if propogation is absent
        var baseResult = HoldingScope!.Resolve<TOut>(input, outputType, range, propogate);

        // End propogation here if necessary
        if(!propogate) return baseResult;

        // Accumulate all non-null results into this list
        var includedResults = ResultGroup.Foreach(IncludedNamespaces, n => n.Resolve<TOut>(input, outputType, range, false)).CombineSkip();

        var allResults = includedResults.Value.ToList();

        if(baseResult) 
            allResults.Add(baseResult.Value);

        var builder = ResultBuilder
            .From(includedResults);

        if(baseResult) builder
            .AddData(baseResult);

        // Return normal circumstances
        if(allResults.Count == 0) return builder.WithDgnError($"Could not find {outputType} named {input}", range).BuildError<TOut>();

        if(allResults.Count == 1) return builder.Build(allResults.First());

        // If allowed, produce an ambiguous resolution error
        return builder.WithDgnError(
            "Ambiguity between " + 
            string.Join(", ", allResults.Select(t => t.FullName)) +
            "; either disambiguate, remove includes, or use explicit '.' accesses"
        , range).BuildError<TOut>();
    }

    public override Result<T> Define<T>(Func<IScope, T> constructor, SourceRange sourceRange)
        => HoldingScope!.Define(constructor, sourceRange);
}
