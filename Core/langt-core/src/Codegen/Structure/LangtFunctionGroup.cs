using Langt.AST;

namespace Langt.Codegen;

public record LangtFunctionGroup(string Name) : INamedScoped
{
    public string DisplayName => CodeGenerator.DisplayableFunctionGroupName(Name);

    public record struct Resolution(LangtFunction Function, Result<BoundASTNode[]> OutputParameters, SignatureMatchLevel MatchLevel);

    public LangtScope? HoldingScope { get; set; }

    private readonly List<LangtFunction> overloads = new();
    public IReadOnlyList<LangtFunction> Overloads => overloads;

    public Result AddFunctionOverload(LangtFunction overload, SourceRange range)
    {
        var r = ResolveExactOverload(overload.Type.ParameterTypes, overload.Type.IsVararg, range);

        if(r) return Result.Error
        (
            Diagnostic.Error($"Cannot redefine overload of function {this.GetFullName()} with signature {overload.Type.SignatureString}", range)
        );

        overloads.Add(overload);
        return Result.Success();
    }

    private Result<Resolution> HandleOverload(Resolution[] resolves, SourceRange range) 
    {
        var builder = ResultBuilder.Empty();

        if(resolves.Length == 0) 
        {
            return builder.WithDgnError(
                $"Could not resolve any matching overloads for call to {this.GetFullName()}",
                range
            ).Build<Resolution>();
        }

        if(resolves.Length == 1)
        {
            return builder.Build(resolves[0]);
        }

        for(var lvl = SignatureMatchLevel.Exact; lvl != SignatureMatchLevel.None; lvl++)
        {
            var byLevel = resolves.Where(o => o.MatchLevel == lvl).ToArray();

            if(byLevel.Length == 0) continue;

            if(byLevel.Length != 1)
            {
                return builder.WithDgnError(
                    $"Could not resolve any one matching overload for call to {this.GetFullName()}, multiple overloads are valid",
                    range
                ).Build<Resolution>();
            }
            else
            {
                return builder.Build(byLevel[0]);
            }
        }

        throw new UnreachableException("This point should never be reached.");
    }

    public Result<Resolution> ResolveOverload(ASTNode[] parameters, SourceRange range, ASTPassState state) 
    {
        var resolvesFirst = overloads
            .Select(o => new {Value = o, Resolution = o.Type.MatchSignature(state, range, parameters)})
            .ToArray();

        if(resolvesFirst.FirstOrDefault(p => p.Resolution.InternalError) is var p && p is not null) 
            return p.Resolution.OutResult.Cast<Resolution>();

        var resolves = resolvesFirst
            .Where(r => !r.Resolution.OutResult.HasErrors)
            .Select(r => new Resolution(r.Value, r.Resolution.OutResult, r.Resolution.Level))
            .ToArray();

        return HandleOverload(
            resolves,
            range
        );
    }
    public Result<Resolution> ResolveExactOverload(LangtType[] parameterTypes, bool isVararg, SourceRange range) 
    {
        var resolves = overloads
            .Where(o => o.Type.ParameterTypes.SequenceEqual(parameterTypes) && o.Type.IsVararg == isVararg)
            .Select(o => new Resolution(o, Result.Blank<BoundASTNode[]>(), SignatureMatchLevel.Exact))
            .ToArray();
        
        return HandleOverload(
            resolves, 
            range
        );
    }
}
