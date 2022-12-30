using Langt.AST;
using Langt.Codegen;

namespace Langt.Codegen;

public record LangtFunctionType(LangtType ReturnType, bool IsVararg, LangtType[] ParameterTypes) : LangtType(GetName(ReturnType, ParameterTypes, IsVararg))
{
    public record struct ResolutionResult(Result<BoundASTNode[]> OutResult, SignatureMatchLevel Level, bool InternalError);

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

    public ResolutionResult MatchSignature(ASTPassState state, SourceRange range, ASTNode[] parameters)
    {
        var level = SignatureMatchLevel.None;

        // less params given than required           || more params given than specified if not vararg
        if(parameters.Length < ParameterTypes.Length || (!IsVararg && parameters.Length > ParameterTypes.Length)) 
        {
            return new
            (
                Result.Error<BoundASTNode[]>
                (
                    Diagnostic.Error("Incorrect number of parameters", range)
                ),
                SignatureMatchLevel.None,
                false
            );
        }

        level = SignatureMatchLevel.Exact;
        bool internalErr = false;
        Result<BoundASTNode>? internalErrRes = null;

        var group = ResultGroup.Foreach
        (
            parameters.Indexed(),
            p => 
            {
                if(p.Index >= ParameterTypes.Length)
                {
                    var r = p.Value.Bind(state);
                    
                    if(!r) 
                    {
                        internalErr = true;
                        internalErrRes = r;
                    }

                    return r;
                }

                var op = p.Value.BindMatching(state, ParameterTypes[p.Index], out var coerced, out internalErr);
                if(internalErr)
                {
                    internalErrRes = op;
                }

                var clevel = coerced ? SignatureMatchLevel.Coerced : SignatureMatchLevel.Exact;

                if(level < clevel)
                {
                    level = clevel;
                }

                return op;
            }
        );

        if(internalErr)
        {
            return new(internalErrRes!.Value.Cast<BoundASTNode[]>(), level, true);
        }
        else 
        {
            return new(group.Combine().Map(k => k.ToArray()), level, false);
        }
    }
}
