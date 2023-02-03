using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerDereferenceAssignment : ILowerImplementation<BoundDereferenceAssignment>
{
    public void LowerImpl(CodeGenerator cg, BoundDereferenceAssignment node)
    {
        cg.Lower(node.LeftValue);
        var l = cg.PopValue(node.DebugSourceName);

        cg.Lower(node.Right);
        var r = cg.PopValue(node.DebugSourceName);

        cg.Builder.BuildStore(r, l);
    }
}