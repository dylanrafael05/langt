using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;

public class LangtFunction : Resolution
{
    public LangtFunction(LangtFunctionGroup group) : base(group.HoldingScope)
    {
        Group = group;
    }

    public LangtFunctionGroup Group {get;}

    public required LangtFunctionType Type {get; init;}
    public required string[] ParameterNames {get; init;}
    public required LLVMValueRef LLVMFunction {get; init;}

    public override string Name => Group.Name;
}