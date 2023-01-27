using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerDereference : ILowerImplementation<BoundDereference>
{
    public void LowerImpl(CodeGenerator cg, BoundDereference node)
    {
        cg.Lower(node.Value);
        var f = cg.PopValue(node.DebugSourceName);

        cg.PushValue
        (
            f.Type.ElementType!,
            cg.Builder.BuildLoad2(cg.Binder.Get(f.Type.ElementType!), f),
            node.DebugSourceName
        );
    }
}
