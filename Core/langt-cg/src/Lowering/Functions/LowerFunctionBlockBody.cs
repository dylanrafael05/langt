using Langt.AST;
using Langt.Structure;

namespace Langt.CG.Lowering;

public struct LowerFunctionBlockBody : ILowerImplementation<BoundFunctionBlockBody>
{
    public void LowerImpl(CodeGenerator cg, BoundFunctionBlockBody node)
    {
        cg.Lower(node.Body);

        if(cg.CurrentFunctionType.ReturnType == LangtType.None && !node.Body.Returns)
        {
            cg.Builder.BuildRetVoid();
        }
    }
}
