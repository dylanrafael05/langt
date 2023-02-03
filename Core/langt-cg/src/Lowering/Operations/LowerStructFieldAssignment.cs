using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerStructFieldAssignment : ILowerImplementation<BoundStructFieldAssignment>
{
    public void LowerImpl(CodeGenerator cg, BoundStructFieldAssignment node)
    {
        var l = LowerHelpers.BuildStructGEP(cg, node, node.FieldIndex, node.Field, node.DebugSourceName);
        
        cg.Lower(node.Right);
        var r = cg.PopValue(node.DebugSourceName);

        cg.Builder.BuildStore(r, l);
    }
}

