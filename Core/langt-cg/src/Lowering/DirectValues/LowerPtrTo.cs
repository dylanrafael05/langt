using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerPtrTo : ILowerImplementation<BoundPtrTo>
{
    public void LowerImpl(CodeGenerator cg, BoundPtrTo node)
    {
        cg.Lower(node.Value);
        var f = cg.PopValue(node.DebugSourceName);
        cg.PushValue
        ( 
            f.Type, 
            f,
            node.DebugSourceName
        );
    }
}
