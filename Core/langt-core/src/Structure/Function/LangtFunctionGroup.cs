using Langt.AST;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtFunctionGroup : Resolvable
{
    public LangtFunctionGroup(string name, IScope scope) : base(scope)
    {
        Name = name;
    }

    public override string Name {get;}
    public override string DisplayName => Context.DisplayableFunctionGroupName(Name);

    public record struct Resolution(LangtFunction Function, Result<BoundASTNode[]> OutputParameters, SignatureMatchLevel MatchLevel);

    private readonly List<LangtFunction> overloads = new();
    public IReadOnlyList<LangtFunction> Overloads => overloads;

    public Result AddFunctionOverload(LangtFunction overload, SourceRange range)
    {
        if(overload.Group != this) throw new InvalidDataException("Cannot add a function overload to a group other than the one it is defined for.");

        var r = ResolveExactOverload(overload.Type.ParameterTypes, overload.Type.IsVararg, range);

        if(r) return Result.Error
        (
            Diagnostic.Error($"Cannot redefine overload of function {FullName} with signature {overload.Type.SignatureString}", range)
        );

        overloads.Add(overload);
        return Result.Success();
    }

    private Result<Resolution> HandleOverload(Resolution[] resolves, SourceRange range, IEnumerable<LangtType?> parameterTypes) 
    {
        string Types() => "type".Pluralize(parameterTypes.Count()) + parameterTypes.Stringify();

        var builder = ResultBuilder.Empty();

        if(resolves.Length == 0) 
        {
            return builder.WithDgnError(
                $"Could not resolve any matching overloads for call to {FullName} with parameter {Types()}",
                range
            ).BuildError<Resolution>();
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
                    $"Could not resolve any one matching overload for call to {FullName} with parameter {Types()}, multiple overloads are valid",
                    range
                ).BuildError<Resolution>();
            }
            else
            {
                return builder.Build(byLevel[0]);
            }
        }

        throw new UnreachableException("This point should never be reached.");
    }

    public Result<Resolution> ResolveOverload(ASTNode[] parameters, SourceRange range, Context ctx) 
    {
        var mmsi = LangtFunctionType.MutableMatchSignatureInput.From(parameters);

        var resolvesFirst = overloads
            .Select(o => new {Value = o, Resolution = o.Type.MatchSignature(ctx, range, mmsi)})
            .ToArray();

        if(resolvesFirst.FirstOrDefault(p => p.Resolution.OutResult.HasErrors && !p.Resolution.OutResult.GetBindingOptions().TargetTypeDependent) is var p 
            && p is not null) 
            return p.Resolution.OutResult.ErrorCast<Resolution>();

        var resolves = resolvesFirst
            .Where(r => !r.Resolution.OutResult.HasErrors)
            .Select(r => new Resolution(r.Value, r.Resolution.OutResult, r.Resolution.Level))
            .ToArray();

        return HandleOverload(
            resolves,
            range,
            mmsi.BoundParameters.Where(k => k.HasValue)
                .Select(k => k!.Value)
                .Select(k => k.HasValue ? k.Value.Type : LangtType.Error)
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
            range,
            parameterTypes
        );
    }
}
