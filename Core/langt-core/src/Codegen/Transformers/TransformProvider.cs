using Langt.Codegen;

namespace Langt.AST;

public abstract class TransformProvider
{
    public virtual LangtType? DirectInput {get;}
    public virtual LangtType? DirectResult {get;}

    public abstract string Name {get;}

    public bool IsDirect => DirectInput is not null && DirectResult is not null;

    public virtual bool CanPerform(LangtType input, LangtType result) => input == DirectInput && result == DirectResult;
    public abstract LLVMValueRef Perform(LangtType input, LangtType target, CodeGenerator generator, LLVMValueRef value);

    public ITransformer TransformerFor(LangtType input, LangtType result)
        => new FunctionalTransformer(input, result, (cg, v) => Perform(input, result, cg, v));
}
