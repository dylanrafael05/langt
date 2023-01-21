using Langt.AST;
using Langt.Structure;

namespace Langt.CG.Lowering;

public struct LowerBoundGroup : ILowerImplementation<BoundGroup>
{
    public void LowerImpl(CodeGenerator cg, BoundGroup node)
    {
        if(node.HasScope)
        {
            foreach(var variable in node.Scope!.NamedItems.Values.OfType<LangtVariable>())
            {
                var llvm = cg.Builder.BuildAlloca(cg.Binder.Get(variable.Type), "var."+variable.Name);
                
                if(variable.IsParameter)
                {
                    cg.Builder.BuildStore
                    (
                        cg.CurrentFunction!.LLVMFunction.GetParam(variable.ParameterNumber!.Value), llvm
                    );
                }

                cg.Binder.BindVariable(variable, llvm);
            }
        }

        foreach(var s in node.BoundNodes)
        {
            cg.Lower(s);
            cg.DiscardValues(node.DebugSourceName);
        }
    }
}
