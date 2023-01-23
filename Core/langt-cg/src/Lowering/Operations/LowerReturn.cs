using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerReturn : ILowerImplementation<BoundReturn>
{
    public void LowerImpl(CodeGenerator cg, BoundReturn node)
    {
        if(node.Value is null)
        {
            cg.Builder.BuildRetVoid();
        }
        else
        {
            cg.Lower(node.Value);
            cg.Builder.BuildRet(cg.PopValue(node.DebugSourceName));
        }
    }
}