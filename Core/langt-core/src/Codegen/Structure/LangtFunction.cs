using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;

public record LangtFunction(LangtFunctionType Type, string[] ParameterNames, LLVMValueRef LLVMFunction)
{
    public LangtScope? HoldingScope { get; set; }
}