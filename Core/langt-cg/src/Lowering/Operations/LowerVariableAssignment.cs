using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerVariableAssignment : ILowerImplementation<BoundVariableAssignment>
{
    public void LowerImpl(CodeGenerator cg, BoundVariableAssignment node)
    {
        var l = cg.Binder.Get(node.Variable);
        
        cg.Lower(node.Right);
        var r = cg.PopValue(node.DebugSourceName);

        cg.Builder.BuildStore(r, l);
    }
}
