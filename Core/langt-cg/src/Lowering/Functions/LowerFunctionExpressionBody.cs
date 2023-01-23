using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerFunctionExpressionBody : ILowerImplementation<BoundFunctionExpressionBody>
{
    public void LowerImpl(CodeGenerator cg, BoundFunctionExpressionBody node)
    {
        cg.Lower(node.Expression);
        cg.Builder.BuildRet(cg.PopValue(node.DebugSourceName));
    }
}