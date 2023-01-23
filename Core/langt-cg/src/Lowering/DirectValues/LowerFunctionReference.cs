using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerFunctionReference : ILowerImplementation<BoundFunctionReference>
{
    public void LowerImpl(CodeGenerator cg, BoundFunctionReference node)
    {
        cg.PushValue( 
            node.Function.Type, 
            cg.Binder.Get(node.Function),
            node.DebugSourceName
        );
    }
}