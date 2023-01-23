using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerStructInitializer : ILowerImplementation<BoundStructInitializer>
{
    public void LowerImpl(CodeGenerator cg, BoundStructInitializer node)
    {
        var llvmArgs = new List<LLVMValueRef>();

        foreach(var arg in node.BoundArgs)
        {
            cg.Lower(arg);
            llvmArgs.Add(cg.PopValue(node.DebugSourceName).LLVM);
        }

        var structBuild = cg.Builder.BuildAlloca(cg.Binder.Get(node.Type), node.Type.Name+".builder");

        for(int i = 0; i < llvmArgs.Count; i++)
        {
            var fptr = cg.Builder.BuildStructGEP2(cg.Binder.Get(node.Type), structBuild, (uint)i, node.Type.Name+".builder.element."+i);
            cg.Builder.BuildStore(llvmArgs[i], fptr);
        }

        cg.PushValue( 
            node.Type,
            cg.Builder.BuildLoad2(cg.Binder.Get(node.Type), structBuild, node.Type.Name),
            node.DebugSourceName
        );
    }
}
