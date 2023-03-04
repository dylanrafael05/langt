using System.Diagnostics.CodeAnalysis;
using Langt.Structure;


namespace Langt.Structure;

public class LangtVariable : ImmediateResolvable
{
    [SetsRequiredMembers]
    public LangtVariable(string name, LangtType type, IScope scope)
    {
        Name = name;
        Type = type;
        HoldingScope = scope;
    }

    public LangtType Type {get; init;}

    public bool IsWriteable {get; init;} = true;
    public uint? ParameterNumber {get; init;}

    public int UseCount {get; set;} = 0;

    public bool IsParameter => ParameterNumber is not null;
}