using Langt.AST;
using Langt.Structure;

namespace Langt.CG.Lowering;

public struct LowerSizeof : ILowerImplementation<BoundSizeof>
{
    public void LowerImpl(CodeGenerator cg, BoundSizeof node)
    {
        cg.PushValue(
            LangtType.UIntSZ,
            LLVMValueRef.CreateConstInt(cg.Binder.Get(LangtType.UIntSZ), cg.Sizeof(node.SizeofType)),
            node.DebugSourceName
        );
    }
}