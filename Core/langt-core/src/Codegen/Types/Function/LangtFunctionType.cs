using Langt.AST;
using Langt.Codegen;

namespace Langt.Codegen;

public record LangtFunctionType(LangtType ReturnType, bool IsVararg, LangtType[] ParameterTypes) : LangtType(GetName(ReturnType, ParameterTypes, IsVararg))
{
    public static string GetName(LangtType ret, LangtType[] param, bool varg) 
    {
        return "(" + string.Join(",", param.Select(p => p.GetFullName())) + (varg ? " ..." : "") + ")" + ret.GetFullName();
    }

    public static string GetSignatureString(bool isVararg, params LangtType[] paramTypes)
    {
        return string.Join(",", paramTypes.Select(p => p.GetFullName())) + (isVararg ? ",..." : "");
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

    public bool SignatureMatches(ASTNode[] parameters, CodeGenerator generator, out ITransformer?[]? transformers, out SignatureMatchLevel level)
    {
        transformers = null;
        level = SignatureMatchLevel.None;

        if(parameters.Length < ParameterTypes.Length) return false;
        if(!IsVararg && parameters.Length != ParameterTypes.Length) return false;

        var l = new List<ITransformer?>();

        level = SignatureMatchLevel.Exact;

        for(var i = 0; i < ParameterTypes.Length; i++)
        {
            if(!generator.CanMatch(ParameterTypes[i], parameters[i].TransformedType, parameters[i].Readable, out var t))
            {
                return false;
            }

            if(t is not null) level = SignatureMatchLevel.Coerced;

            l.Add(t);
        }

        transformers = l.ToArray();
        return true;
    }
}
