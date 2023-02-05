using Langt.Structure;

namespace Langt.CG.Bindings;

public class FunctionBuilder : Builder<LangtFunction, LLVMValueRef>
{
    public override LLVMValueRef Build(LangtFunction fn)
    {
        return CG.Module.AddFunction
        (
            fn.IsExtern ? fn.Name : fn.MangledName(), 
            CG.Binder.Get(fn.Type)
        );
    }
}
