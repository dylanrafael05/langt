using Langt.Codegen;

namespace Langt.AST;

public record FunctionalTransformer(LangtType Input, LangtType Output, TransformFunction TransformFunction) : ITransformer
{
    public LLVMValueRef Perform(CodeGenerator generator, LLVMValueRef value) => TransformFunction(generator, value);
}
