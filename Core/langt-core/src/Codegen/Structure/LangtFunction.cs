using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;

public class LangtFunction : NamedBase
{
    public LangtFunction(LangtFunctionGroup group)
    {
        Group = group;
    }

    public LangtFunctionGroup Group {get;}

    public required LangtFunctionType Type {get; init;}
    public required string[] ParameterNames {get; init;}
    public required LLVMValueRef LLVMFunction {get; init;}

    public override string Name => Group.RawName;

    public LangtScope HoldingScope => Group.HoldingScope;
    public SourceRange? DefinitionRange {get; init;}
    public string Documentation {get; init;} = "";
}