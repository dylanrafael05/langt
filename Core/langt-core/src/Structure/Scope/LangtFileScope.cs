using Langt.AST;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtFileScope : LangtScope
{
    public LangtFileScope(IScope scope) : base(scope)
    {
        Expect.ArgNonNull(scope, "File scopes must have an enclosing scope!");
    }

    // A list of namespaces included by the source code with 'using blah.blah.blah' directives
    public List<LangtNamespace> IncludedNamespaces {get; init;} = new();

    public override Result<TOut>? ResolveSelf<TOut>(string input, string outputType, ASTPassState state, SourceRange range, TypeCheckOptions? options = null)
    {
        return HoldingScope!.ResolveSelfPossibly<TOut>(input, outputType, state, range, options);
    }

    public override Result<TOut> Resolve<TOut>(string input, string outputType, ASTPassState state, SourceRange range, TypeCheckOptions? optionsMaybe = null)
    {
        // Get basic result, allowing errors if propogation is absent
        var baseResult = ResolveSelf<TOut>(input, outputType, state, range, optionsMaybe);

        // Error out if something has gone wrong
        if(baseResult.HasValue && !baseResult.Value)
            return baseResult.Value;

        // Accumulate all non-null results into this list
        var includedResults = ResultGroup.Foreach(IncludedNamespaces, n => n.Resolve<TOut>(input, outputType, state, range, optionsMaybe)).CombineSkip();

        var allResults = includedResults.Value.ToList();

        if(baseResult.HasValue) 
            allResults.Add(baseResult.Value.Value);

        var builder = ResultBuilder.From(includedResults);

        if(baseResult.HasValue)
            builder.AddData(baseResult.Value);

        // Return normal circumstances
        if(allResults.Count == 0) return builder.WithDgnError($"Could not find {outputType} named {input}", range).BuildError<TOut>();

        if(allResults.Count == 1) return builder.Build(allResults.First());

        // If allowed, produce an ambiguous resolution error
        return builder.WithDgnError(
            "Ambiguity between " + 
            string.Join(", ", allResults.Select(t => )) +
            "; either disambiguate, remove includes, or use explicit '.' accesses"
        , range).BuildError<TOut>();
    }

    public override Result<T> Define<T>(Func<IScope, T> constructor, SourceRange sourceRange)
        => HoldingScope!.Define(constructor, sourceRange);
}
