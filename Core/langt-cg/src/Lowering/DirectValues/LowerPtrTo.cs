using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerPtrTo : ILowerImplementation<BoundPtrTo>
{
    public void LowerImpl(CodeGenerator cg, BoundPtrTo node)
    {
        var f = cg.Binder.Get(node.Variable);
        cg.PushValue
        ( 
            node.Type, 
            f,
            node.DebugSourceName
        );
    }
}
