using Langt.AST;
using Langt.Structure;
using Langt.Structure.Resolutions;

namespace Langt.CG.Lowering;

public struct LowerBoundGroup : ILowerImplementation<BoundGroup>
{
    public void LowerImpl(CodeGenerator cg, BoundGroup node)
    {
        if(node.HasScope)
        {
            cg.InitializeScope(node.Scope);
        }

        foreach(var s in node.BoundNodes)
        {
            cg.Lower(s);
            cg.DiscardValues(node.DebugSourceName);
        }
    }
}
