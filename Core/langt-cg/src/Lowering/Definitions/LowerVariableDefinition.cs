using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerVariableDefinition : ILowerImplementation<BoundVariableDefinition>
{
    public void LowerImpl(CodeGenerator cg, BoundVariableDefinition node)
    {
        if(node.Variable.UseCount > 0)
        {
            cg.Lower(node.Value);
            cg.Builder.BuildStore(cg.PopValue(node.DebugSourceName).LLVM, cg.Binder.Get(node.Variable));
        }
    }
}
