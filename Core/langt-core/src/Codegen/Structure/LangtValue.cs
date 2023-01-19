using Langt.Codegen;

namespace Langt.Codegen;

public record LangtValue(LangtType Type, LLVMValueRef LLVM)
{
    public static implicit operator LLVMValueRef(LangtValue v) => v.LLVM;
}
