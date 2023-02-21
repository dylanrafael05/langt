using System.Diagnostics.CodeAnalysis;
using Langt.Structure;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtVariable : Resolvable
{
    public LangtVariable(string name, LangtType type, IScope scope) : base(scope)
    {
        Name = name;
        Type = type;
    }

    public override string Name {get;}
    public LangtType Type {get; init;}

    public bool IsWriteable {get; init;} = true;
    public uint? ParameterNumber {get; init;}

    public int UseCount {get; set;} = 0;

    public bool IsParameter => ParameterNumber is not null;
}