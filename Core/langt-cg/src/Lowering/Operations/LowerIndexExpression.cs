using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerIndexExpression : ILowerImplementation<BoundIndexExpression>
{
    public void LowerImpl(CodeGenerator cg, BoundIndexExpression node)
    {
        cg.Lower(node.Value);
        var val = cg.PopValue(node.DebugSourceName);

        cg.Lower(node.Index);
        var index = cg.PopValue(node.DebugSourceName);

        var pointeeType = cg.Binder.Get(node.Value.Type.ElementType!);

        cg.PushValue( 
            val.Type,
            cg.Builder.BuildGEP2(pointeeType, val, new LLVMValueRef[] {index}, "index." + val.Type.Name),
            node.DebugSourceName
        );
    }
}