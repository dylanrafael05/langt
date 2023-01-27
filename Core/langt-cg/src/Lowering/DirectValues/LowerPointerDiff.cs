using Langt.AST;
using Langt.Structure;

namespace Langt.CG.Lowering;

public struct LowerPointerDiff : ILowerImplementation<BoundPointerDiff>
{
    public void LowerImpl(CodeGenerator cg, BoundPointerDiff node)
    {
        cg.Lower(node.Left);
        cg.Lower(node.Right);

        var (r, l) = (cg.PopValue(node.DebugSourceName), cg.PopValue(node.DebugSourceName));

        cg.PushValue(
            LangtType.IntSZ,
            cg.Builder.BuildPtrDiff2(cg.Binder.Get(l.Type.ElementType!), l, r),
            node.DebugSourceName
        );
    }
}