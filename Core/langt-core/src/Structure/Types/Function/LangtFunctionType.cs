using Langt.AST;
using Langt.Structure;

namespace Langt.Structure;

// TODO: continue implementing non-record types
// * Make constructor functions return a Result 
// * (to report errors like 'none' parameters or 'none' pointer types)
// * and have them take in a source range.
// * Create "forcing" variants to allow for pre-built types.

public class FunctionTypeSymbol : Symbol<LangtType>
{
    public required ISymbol<LangtType> ReturnType {get; init;}
    public required ISymbol<LangtType>[] ParameterTypes {get; init;}
    public required bool IsVararg {get; init;}

    public string[]? ParameterNames {get; init;}

    public override Result<LangtType> Unravel(Context ctx)
    {
        var builder = ResultBuilder.Empty();

        var retRes = ReturnType.Unravel(ctx);
        builder.AddData(retRes);

        var retTy = retRes.Or(LangtType.Error);

        var parameterTypes = new List<LangtType>();

        foreach(var paramSym in ParameterTypes)
        {
            var paramRes = paramSym.Unravel(ctx);
            builder.AddData(paramRes);

            parameterTypes.Add(paramRes.Or(LangtType.Error));
        }

        return builder.Build<LangtType>(new LangtFunctionType(retTy, parameterTypes.ToArray(), IsVararg));
    }
}

public class LangtFunctionType : LangtType
{
    public LangtFunctionType(LangtType ret, LangtType[] param, bool varg) 
    {
        ReturnType = ret;
        ParameterTypes = param;
        IsVararg = varg;
    }

    public override bool Equals(LangtType? other)
        => other is not null
        && other.IsFunction
        && ParameterTypes.SequenceEqual(other.Function.ParameterTypes)
        && ReturnType == other.Function.ReturnType
        && IsVararg == other.Function.IsVararg;

    public override LangtType ReplaceGeneric(LangtType gen, LangtType rep)
    {
        var changed = false;

        LangtType? retTy = null;
        LangtType[]? pTys = null;

        if(gen == ReturnType)
        {
            changed = true;
            retTy = rep;
        }

        if(ParameterTypes.Contains(rep))
        {
            changed = true;
            pTys = ParameterTypes.Select(x => x.ReplaceGeneric(gen, rep)).ToArray();
        }

        if(changed) return new LangtFunctionType(retTy ?? ReturnType, pTys ?? ParameterTypes, IsVararg);

        return this;
    }

    public LangtType ReturnType {get; private init;}
    public LangtType[] ParameterTypes {get; private init;}
    public bool IsVararg {get; private init;}

    public override string Name => GetName(ReturnType, ParameterTypes, IsVararg);
    public override string FullName => Name;

    public record struct MatchSignatureResult(Result<BoundASTNode[]> OutResult, SignatureMatchLevel Level);
    public class MutableMatchSignatureInput
    {
        private MutableMatchSignatureInput(ASTNode[] parameters)
        {
            Parameters = parameters;
            CachedBoundParameters = new Result<BoundASTNode>?[parameters.Length];
        }
        public static MutableMatchSignatureInput From(ASTNode[] parameters) => new(parameters);

        private ASTNode[] Parameters {get; init;}
        private Result<BoundASTNode>?[] CachedBoundParameters {get; init;}

        public IList<Result<BoundASTNode>?> BoundParameters => CachedBoundParameters;
        
        public int ParameterCount => Parameters.Length;

        public Result<BoundASTNode> GetBoundTo(int n, Context ctx, LangtType? t, out bool coerced)
        {
            Result<BoundASTNode> current;

            if(CachedBoundParameters[n] is null)
            {
                current = Parameters[n].Bind(ctx, new TypeCheckOptions {TargetType = t});
                
                if(!current.GetBindingOptions().TargetTypeDependent)
                    CachedBoundParameters[n] = current;
            }
            else 
            {
                current = CachedBoundParameters[n]!.Value;
            }

            coerced = false;
            if(t is null || !current) return current;
            return current.Value.MatchExprType(ctx, t, out coerced);
        }

        public Result<BoundASTNode> Get(int n, Context ctx) 
            => GetBoundTo(n, ctx, null, out _);
    }

    public static string GetName(LangtType ret, LangtType[] param, bool varg) 
    {
        return GetFullSignatureString(varg, param) + " " + ret.FullName;
    }

    public static string GetFullSignatureString(bool isVararg = false, params LangtType[] paramTypes)
    {
        return "("+string.Join(", ", paramTypes.Select(p => p.FullName)) + (isVararg ? ", ..." : "")+")";
    }
    public static string GetSignatureString(bool isVararg, params LangtType[] paramTypes)
    {
        return "("+string.Join(", ", paramTypes.Select(p => p.FullName)) + (isVararg ? ", ..." : "")+")";
    }

    public string SignatureString => GetFullSignatureString(IsVararg, ParameterTypes);

    public MatchSignatureResult MatchSignature(Context ctx, SourceRange range, MutableMatchSignatureInput ipt)
    {
        var level = SignatureMatchLevel.None;

        // less params given than required            || more params given than specified if not vararg
        if(ipt.ParameterCount < ParameterTypes.Length || (!IsVararg && ipt.ParameterCount > ParameterTypes.Length)) 
        {
            return new
            (
                Result.Error<BoundASTNode[]>
                (
                    Diagnostic.Error("Incorrect number of parameters", range)
                ),
                SignatureMatchLevel.None
            );
        }

        level = SignatureMatchLevel.Exact;

        var group = ResultGroup.Foreach
        (
            Enumerable.Range(0, ipt.ParameterCount),
            index => 
            {
                if(index >= ParameterTypes.Length)
                {
                    var r = ipt.Get(index, ctx);

                    return r;
                }

                var op = ipt.GetBoundTo(index, ctx, ParameterTypes[index], out var coerced);

                var clevel = coerced ? SignatureMatchLevel.Coerced : SignatureMatchLevel.Exact;

                if(level < clevel)
                {
                    level = clevel;
                }

                return op;
            }
        );

        return new(group.Combine().Map(k => k.ToArray()), level);
    }

    public MatchSignatureResult MatchSignature(Context ctx, SourceRange range, ASTNode[] parameters)
        => MatchSignature(ctx, range, MutableMatchSignatureInput.From(parameters));
}
