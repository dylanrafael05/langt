using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerFunctionPointerCall : ILowerImplementation<BoundFunctionPointerCall>
{
    public void LowerImpl(CodeGenerator cg, BoundFunctionPointerCall node)
    {
        cg.Lower(node.Function);

        cg.BuildFunctionCall
        (
            cg.PopValue(node.DebugSourceName).LLVM,
            node.Arguments,
            node.FunctionType,
            node.DebugSourceName
        );
    }
}
