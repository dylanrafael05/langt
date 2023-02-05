using Langt.CG;
using Langt.Structure;

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

        if(!engine.TryFindFunction("_L1X4main0", out var f))
        {
            logger.Error("No function named 'main' found in given code.");
            return;
        }

        engine.RunFunction(f, Array.Empty<LLVMGenericValueRef>());
    }
}