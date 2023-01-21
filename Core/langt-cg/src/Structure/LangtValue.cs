using Langt.Structure;

namespace Langt.CG.Structure;

public record LangtValue(LangtType Type, LLVMValueRef LLVM)
{
    public static implicit operator LLVMValueRef(LangtValue v) => v.LLVM;
}
