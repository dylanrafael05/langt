using Langt.AST;

namespace Langt.Structure;

public class DerefenceTransform : TransformProvider
{
    private DerefenceTransform() {}

    public override bool CanPerform(LangtType input, LangtType result) => input.IsReference && result == input.ElementType;
    public override LLVMValueRef Perform(LangtType input, LangtType result, Context generator, LLVMValueRef value)
    {
        return generator.Builder.BuildLoad2(generator.LowerType(result), value, "read."+input.Name);
    }

    public override string Name => "&a->a";

    public static ITransformer For(LangtType input) => new DerefenceTransform().TransformerFor(input, input.ElementType!);
}
