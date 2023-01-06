using Langt.AST;
using Langt.Codegen;

namespace Langt.Codegen;

public record LangtFunctionType(LangtType ReturnType, bool IsVararg, LangtType[] ParameterTypes) : LangtType(GetName(ReturnType, ParameterTypes, IsVararg))
{
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
        private Result<BoundASTNode>?[] CachedBoundParameters  {get; init;}
        
        public int ParameterCount => Parameters.Length;

        public Result<BoundASTNode> GetBoundTo(int n, ASTPassState state, LangtType? t, out bool coerced)
        {
            Result<BoundASTNode> current;

            if(CachedBoundParameters[n] is null)
            {
                current = Parameters[n].Bind(state, new TypeCheckOptions {TargetType = t});
                
                if(!current.GetBindingOptions().TargetTypeDependent)
                    CachedBoundParameters[n] = current;
            }
            else 
            {
                current = CachedBoundParameters[n]!.Value;
            }

            coerced = false;
            if(t is null || !current) return current;
            return current.Value.MatchExprType(state, t, out coerced);
        }

        public Result<BoundASTNode> Get(int n, ASTPassState state) 
            => GetBoundTo(n, state, null, out _);
    }

    public static string GetName(LangtType ret, LangtType[] param, bool varg) 
    {
        return GetSignatureString(varg, param) + ret.GetFullName();
    }

    public static string GetSignatureString(bool isVararg, params LangtType[] paramTypes)
    {
        return "("+string.Join(",", paramTypes.Select(p => p.GetFullName())) + (isVararg ? ",..." : "")+")";
    }
    public static string GetSignatureString(params LangtType[] paramTypes)
        => GetSignatureString(false, paramTypes);

    public string SignatureString => GetSignatureString(IsVararg, ParameterTypes);
    
    public LangtFunctionType(LangtType returnType, params LangtType[] parameterTypes) : this(returnType, false, parameterTypes)
    {}

    public override LLVMTypeRef Lower(CodeGenerator context)
        => LLVMTypeRef.CreateFunction(
            context.LowerType(ReturnType),
            ParameterTypes.Select(context.LowerType).ToArray(),
            IsVararg
        );

    public MatchSignatureResult MatchSignature(ASTPassState state, SourceRange range, MutableMatchSignatureInput ipt)
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
                    var r = ipt.Get(index, state);

                    return r;
                }

                var op = ipt.GetBoundTo(index, state, ParameterTypes[index], out var coerced);

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

    public MatchSignatureResult MatchSignature(ASTPassState state, SourceRange range, ASTNode[] parameters)
        => MatchSignature(state, range, MutableMatchSignatureInput.From(parameters));
}
