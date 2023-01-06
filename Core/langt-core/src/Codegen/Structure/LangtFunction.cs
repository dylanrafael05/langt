using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;

public record LangtFunction(LangtFunctionGroup Group, LangtFunctionType Type, string[] ParameterNames, LLVMValueRef LLVMFunction, string Documentation = "") : INamedScoped
{
    public string Name => Group.Name;
    public string DisplayName => Group.DisplayName;
    public LangtScope? HoldingScope  
    {
        get => Group.HoldingScope;
        set => Group.HoldingScope = value; //NOTE: is this problematic?
    }
}