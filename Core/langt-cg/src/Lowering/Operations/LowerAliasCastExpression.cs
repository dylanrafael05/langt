using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerAliasCastExpression : ILowerImplementation<BoundAliasCastExpression>
{
    public void LowerImpl(CodeGenerator cg, BoundAliasCastExpression node)
    {
        cg.Lower(node.Value);
        var s = cg.PopValue(node.DebugSourceName);
        cg.PushValue(node.Type, s, node.DebugSourceName);
    }
}