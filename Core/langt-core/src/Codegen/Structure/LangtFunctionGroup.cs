using Langt.AST;

namespace Langt.Codegen;

public record LangtFunctionGroup(string Name) : INamedScoped
{
    public LangtScope? HoldingScope { get; set; }

    private List<LangtFunction> overloads = new();
    public IReadOnlyList<LangtFunction> Overloads => overloads;

    public void AddFunctionOverload(LangtFunction overload, SourceRange range, DiagnosticCollection diagnostics)
    {
        if(overloads.Any(o => o.Type.ParameterTypes.SequenceEqual(overload.Type.ParameterTypes)))
        {
            diagnostics.Error($"Cannot define an overload which has the same parameter types as another", range);
        }

        overloads.Add(overload);
    }

    public LangtFunction? MatchOverload(ASTNode[] parameters, SourceRange range, CodeGenerator generator)
    {
        var o = ResolveOverload(parameters, range, generator, out var transformers);

        if(o is null) return null;

        for(int i = 0; i < transformers!.Length; i++)
        {
            var t = transformers[i];
            if(t is not null) 
            {
                parameters[i].ApplyTransform(t);
            }
        }

        return o;
    }
    public LangtFunction? ResolveOverload(ASTNode[] parameters, SourceRange range, CodeGenerator generator, out ITransformer?[]? transformers) 
    {
        var resolves = overloads
            .Select(o => new {value = o, matches = o.Type.SignatureMatches(parameters, generator, out var ts, out var lvl), transformers = ts, level = lvl})
            .Where(o => o.matches)
            .Select(o => new {o.value, o.transformers, o.level})
            .ToArray();
        
        if(resolves.Length == 0) 
        {
            generator.Diagnostics.Error(
                $"Could not resolve any matching overload for call to {this.GetFullName()} with types {string.Join(", ", parameters.Select(p => p.ExpressionType.Name))}", 
                range
            );

            transformers = null;
            return null;
        }

        if(resolves.Length == 1)
        {
            transformers = resolves[0].transformers;
            return resolves[0].value;
        }

        for(var lvl = SignatureMatchLevel.Exact; lvl != SignatureMatchLevel.None; lvl++)
        {
            var byLevel = resolves.Where(o => o.level == lvl);

            if(byLevel.Count() != 1)
            {
                generator.Diagnostics.Error(
                    $"Could not resolve any one matching overload for call to {this.GetFullName()} with types {string.Join(", ", parameters.Select(p => p.ExpressionType))}, multiple overloads are valid", 
                    range
                );
            }
            else
            {
                transformers = byLevel.First().transformers;
                return byLevel.First().value;
            }
        }

        transformers = null;
        return null;
    }
}
