using Langt.Structure;

namespace Langt.AST;

public record FunctionalTransformer(LangtType Input, LangtType Output, TransformFunction TransformFunction) : ITransformer
{
    public LLVMValueRef Perform(Context generator, LLVMValueRef value) => TransformFunction(generator, value);
}
