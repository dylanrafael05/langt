using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerNumericLiteral : ILowerImplementation<BoundNumericLiteral>
{
    public void LowerImpl(CodeGenerator cg, BoundNumericLiteral node)
    {
        if(node.DoubleValue.HasValue)
        {
            cg.PushValue( 
                node.Type,
                LLVMValueRef.CreateConstReal(cg.Binder.Get(node.Type), node.DoubleValue!.Value),
                node.DebugSourceName
            );
        }
        else
        {
            cg.PushValue(
                node.Type,
                LLVMValueRef.CreateConstInt(cg.Binder.Get(node.Type), node.IntegerValue!.Value),
                node.DebugSourceName
            );
        }
    }
}