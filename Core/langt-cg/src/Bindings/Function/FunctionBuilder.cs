using Langt.Structure;

namespace Langt.CG.Bindings;

public class FunctionBuilder : Builder<LangtFunction, LLVMValueRef>
{
    public override LLVMValueRef Build(LangtFunction fn)
    {
        return CG.Module.AddFunction
        (
            CodeGenerator.GetFunctionName
            (
                fn.IsExtern,
                fn.Name,
                fn.FullName,
                fn.Type.IsVararg,
                fn.Type.ParameterTypes
            ), 
            CG.Binder.Get(fn.Type)
        );
    }
}
