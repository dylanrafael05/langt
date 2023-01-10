using Langt.AST;

namespace Langt.Codegen;

public class LangtReadPointer : TransformProvider
{
    private LangtReadPointer() {}

    public override bool CanPerform(LangtType input, LangtType result) => input.IsPointer && result == input.PointeeType!;
    public override LLVMValueRef Perform(LangtType input, LangtType result, CodeGenerator generator, LLVMValueRef value)
    {
        return generator.Builder.BuildLoad2(generator.LowerType(input.PointeeType!), value, "read."+input.RawName);
    }

    public override string Name => "readable *a->a";

    public static ITransformer Transformer(LangtType input) => new LangtReadPointer().TransformerFor(input, input.PointeeType!);
}
