using Langt.Codegen;

namespace Langt.AST;

public class DirectTransformProvider : TransformProvider
{
    private readonly LangtType input, output;
    private readonly TransformFunction perform;

    public override LangtType? DirectInput => input;
    public override LangtType? DirectResult => output;

    public override string Name => input.Name + "->" + output.Name;

    public DirectTransformProvider(LangtType output, LangtType input, TransformFunction perform)
    {
        this.input = input;
        this.output = output;
        this.perform = perform;
    }

    public override LLVMValueRef Perform(LangtType input, LangtType output, CodeGenerator generator, LLVMValueRef value) => perform(generator, value);
}