using Langt.Codegen;

namespace Langt.Utility;
public static class LLVMUtil
{
    public static void PrimeLLVM()
    {
        LLVM.LinkInInterpreter();
    }

    public static LLVMTypeRef CreatePointerType(this LLVMContextRef ctx, uint AS) 
        => LLVMTypeRef.CreatePointer(ctx.Int1Type, AS);
    
    public static void CallMain(LLVMModuleRef module, ILogger logger)
    {
        if(!module.TryCreateExecutionEngine(out var engine, out var error))
        {
            logger.Fatal("An error occured while trying to create an execution engine: " + error);
            return;
        }

        if(!engine.TryFindFunction(CodeGenerator.GetGeneratedFunctionName(false, null, "main", false), out var f))
        {
            logger.Error("No function named 'main' found in given code.");
            return;
        }

        engine.RunFunction(f, Array.Empty<LLVMGenericValueRef>());
    }
}