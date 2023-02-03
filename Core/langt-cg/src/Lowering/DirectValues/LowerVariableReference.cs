using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerVariableReference : ILowerImplementation<BoundVariableReference>
{
    public void LowerImpl(CodeGenerator cg, BoundVariableReference node)
    {
        cg.PushValue
        ( 
            node.Type, 
            cg.Builder.BuildLoad2(cg.Binder.Get(node.Type), cg.Binder.Get(node.Variable)),
            node.DebugSourceName
        );
    }
}
