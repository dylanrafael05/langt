using System.Diagnostics.CodeAnalysis;
using Langt.Codegen;

namespace Langt.Codegen;

public record LangtVariable(string Name, LangtType Type, string Documentation = "") : INamedScoped
{
    string INamed.DisplayName => Name;
    
    public bool IsWriteable {get; init;} = true;
    public uint? ParameterNumber {get; init;}

    public int UseCount {get; set;} = 0;

    public bool IsParameter => ParameterNumber is not null;
    public LangtValue? UnderlyingValue {get; private set;}

    public LangtScope? HoldingScope { get; set; }

    public void Attach(LLVMValueRef value)
        => UnderlyingValue = new(Type, value);
    
    protected virtual bool PrintMembers(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"Name = {Name}, Type = {Type}, ");
        stringBuilder.Append($"IsWriteable = {IsWriteable}, ParameterNumber = {ParameterNumber}");
        return true;
    }
}