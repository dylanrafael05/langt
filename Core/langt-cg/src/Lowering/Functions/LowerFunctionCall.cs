using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerFunctionCall : ILowerImplementation<BoundFunctionCall>
{
    public void LowerImpl(CodeGenerator cg, BoundFunctionCall node)
    {
        cg.BuildFunctionCall
        (
            cg.Binder.Get(node.Function),
            node.Arguments,
            node.Function.Type,
            node.DebugSourceName
        );
    }
}