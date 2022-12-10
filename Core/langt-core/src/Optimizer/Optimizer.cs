using Langt.Codegen;

namespace Langt.Optimization ;

public static class Optimizer
{
    public static void Optimize(CodeGenerator generator)
    {
        var m = generator.Module;

        var pass_m = LLVMPassManagerRef.Create();

        pass_m.AddAggressiveDCEPass();
        pass_m.AddFunctionInliningPass();
        pass_m.AddConstantMergePass();
        pass_m.AddInstructionCombiningPass();
        pass_m.AddGlobalOptimizerPass();

        pass_m.Run(m);
        pass_m.Dispose();
    }
}