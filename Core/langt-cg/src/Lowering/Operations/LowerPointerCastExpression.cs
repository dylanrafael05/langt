using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerPointerCastExpression : ILowerImplementation<BoundPointerCastExpression>
{
    public void LowerImpl(CodeGenerator cg, BoundPointerCastExpression node)
    {
        cg.Lower(node.Value);
        var s = cg.PopValue(node.DebugSourceName);
        cg.PushValue(node.Type, s, node.DebugSourceName);
    }
}
