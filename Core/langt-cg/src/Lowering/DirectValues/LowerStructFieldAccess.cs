using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerStructFieldAccess : ILowerImplementation<BoundStructFieldAccess>
{
    public void LowerImpl(CodeGenerator cg, BoundStructFieldAccess node)
    {
        cg.Lower(node);

        var s = cg.PopValue(node.DebugSourceName);

        cg.PushValue( 
            node.Type,
            cg.Builder.BuildStructGEP2(
                cg.Binder.Get(s.Type.ElementType!), 
                s.LLVM,
                (uint)node.FieldIndex!.Value,
                s.Type.ElementType!.Name + "." + node.Field!.Name
            ),
            node.DebugSourceName
        );
    }
}
