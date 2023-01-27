using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerPointerArith : ILowerImplementation<BoundPointerArith>
{
    public void LowerImpl(CodeGenerator cg, BoundPointerArith node)
    {
        cg.Lower(node.Left);
        cg.Lower(node.Right);

        var (r, l) = (cg.PopValue(node.DebugSourceName), cg.PopValue(node.DebugSourceName));

        cg.PushValue(
            l.Type,
            cg.Builder.BuildInBoundsGEP2(cg.Binder.Get(l.Type.ElementType!), l, new[] {r.LLVM}),
            node.DebugSourceName
        );
    }
}