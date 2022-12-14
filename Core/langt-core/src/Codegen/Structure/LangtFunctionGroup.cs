using Langt.AST;

namespace Langt.Codegen;

public record LangtFunctionGroup(string Name) : INamedScoped
{
    public LangtScope? HoldingScope { get; set; }

    private readonly List<LangtFunction> overloads = new();
    public IReadOnlyList<LangtFunction> Overloads => overloads;

    public void AddFunctionOverload(LangtFunction overload, SourceRange range, CodeGenerator generator)
    {
        if(ResolveExactOverload(overload.Type.ParameterTypes, overload.Type.IsVararg, range, generator, err: false) is not null)
        {
            generator.Diagnostics.Error($"Cannot define an overload which has the same parameter types as another", range);
        }

        overloads.Add(overload);
    }

    public LangtFunction? MatchOverload(ASTNode[] parameters, SourceRange range, CodeGenerator generator)
    {
        var o = ResolveOverload(parameters, range, generator, out var matchers);

        if(o is null) return null;

        for(int i = 0; i < matchers!.Length; i++)
        {
            matchers[i].ApplyTo(parameters[i], generator);
        }

        return o;
    }

    private (LangtFunction? value, ASTTypeMatchCreator[]? matchers) HandleOverload(
        (LangtFunction value, ASTTypeMatchCreator[] matchers, SignatureMatchLevel level)[] resolves, 
        IEnumerable<LangtType> parameterTypes,
        SourceRange range,
        CodeGenerator generator,
        bool err = true) 
    {
        if(resolves.Length == 0) 
        {
            if(err) generator.Diagnostics.Error(
                $"Could not resolve any matching overloads for call to {this.GetFullName()} with types {string.Join(", ", parameterTypes.Select(p => p.Name))}", 
                range
            );
            
            return (null, null);
        }

        if(resolves.Length == 1)
        {
            return (resolves[0].value, resolves[0].matchers);
        }

        for(var lvl = SignatureMatchLevel.Exact; lvl != SignatureMatchLevel.None; lvl++)
        {
            var byLevel = resolves.Where(o => o.level == lvl).ToArray();

            if(byLevel.Length != 1)
            {
                if(err) generator.Diagnostics.Error(
                    $"Could not resolve any one matching overload for call to {this.GetFullName()} with parameter types {string.Join(", ", parameterTypes.Select(p => p.Name))}, multiple overloads are valid", 
                    range
                );
            }
            else
            {
                return (byLevel[0].value, byLevel[0].matchers);
            }
        }

        return (null, null);
    }

    public LangtFunction? ResolveOverload(ASTNode[] parameters, SourceRange range, CodeGenerator generator, out ASTTypeMatchCreator[]? matchers, bool err = true) 
    {
        var resolves = overloads
            .Select(o => (value: o, matches: o.Type.SignatureMatches(parameters, generator, out var ts, out var lvl), matchers: ts, level: lvl))
            .Where(o => o.matches)
            .Select(o => (o.value, o.matchers, o.level))
            .ToArray();
        
        (var v, matchers) = HandleOverload(
            resolves, 
            parameters.Select(p => p.TransformedType), 
            range, 
            generator, 
            err
        );

        if(v is not null) 
        {
            generator.Logger.Debug($"Found overload match ({string.Join(",", v.Type.ParameterTypes.Select(p => p.GetFullName()))}) for {this.GetFullName()}@line {range.LineStart}", "lowering");
        }
        else 
        {
            generator.Logger.Debug($"No found overload for {this.GetFullName()}@line {range.LineStart}", "lowering");
        }

        return v;
    }
    public LangtFunction? ResolveExactOverload(LangtType[] parameterTypes, bool isVararg, SourceRange range, CodeGenerator generator, bool err = true) 
    {
        var resolves = overloads
            .Where(o => o.Type.ParameterTypes.SequenceEqual(parameterTypes) && o.Type.IsVararg == isVararg)
            .Select(o => (o, new ASTTypeMatchCreator[o.Type.ParameterTypes.Length], SignatureMatchLevel.Exact))
            .ToArray();
        
        (var v, _) = HandleOverload(
            resolves, 
            parameterTypes, 
            range, 
            generator, 
            err
        );

        return v;
    }
}
