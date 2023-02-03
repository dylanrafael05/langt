using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerStructFieldAccess : ILowerImplementation<BoundStructFieldAccess>
{
    public void LowerImpl(CodeGenerator cg, BoundStructFieldAccess node)
    {
        var (ty, ptr) = LowerHelpers.BuildStructGEP(cg, node, node.FieldIndex, node.Field, node.DebugSourceName);
        cg.PushValue
        (
            ty, 
            cg.Builder.BuildLoad2(cg.Binder.Get(ty.ElementType!), ptr), 
            node.DebugSourceName
        );
    }
}
