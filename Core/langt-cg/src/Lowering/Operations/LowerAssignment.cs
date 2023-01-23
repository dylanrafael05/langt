using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerAssignment : ILowerImplementation<BoundAssignment>
{
    public void LowerImpl(CodeGenerator cg, BoundAssignment node)
    {
        cg.Lower(node.Left);
        cg.Lower(node.Right);

        var (right, left) = (cg.PopValue(node.DebugSourceName), cg.PopValue(node.DebugSourceName));

        cg.Builder.BuildStore(right, left);
    }
}
