using Langt.Structure;

namespace Langt.AST;

public class FunctionalTransformProvider : TransformProvider
{
    private readonly TransformCanPerformPredicate canPerform;
    private readonly TransformProviderFunction perform;
    private readonly string name;
    
    public override string Name => name;

    public FunctionalTransformProvider(TransformCanPerformPredicate canPerform, TransformProviderFunction perform, string name)
    {
        this.canPerform = canPerform;
        this.perform = perform;
        this.name = name;
    }

    public override bool CanPerform(LangtType input, LangtType output) => canPerform(input, output);
    public override LLVMValueRef Perform(LangtType input, LangtType output, Context generator, LLVMValueRef value) => perform(input, output, generator, value);
}
