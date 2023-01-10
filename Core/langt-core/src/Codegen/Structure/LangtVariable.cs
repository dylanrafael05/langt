using System.Diagnostics.CodeAnalysis;
using Langt.Codegen;

namespace Langt.Codegen;

public class LangtVariable : NamedScopedBase
{
    public LangtVariable(string name, LangtType type, string documentation = "") : base(documentation)
    {
        Name = name;
        Type = type;
    }

    public override string Name {get; init;}
    public LangtType Type {get; init;}

    public bool IsWriteable {get; init;} = true;
    public uint? ParameterNumber {get; init;}

    public int UseCount {get; set;} = 0;

    public bool IsParameter => ParameterNumber is not null;
    public LangtValue? UnderlyingValue {get; private set;}

    public void Attach(LLVMValueRef value)
        => UnderlyingValue = new(Type, value);
}