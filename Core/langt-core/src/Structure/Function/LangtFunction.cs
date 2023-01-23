using System.Diagnostics.CodeAnalysis;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtFunction : Resolution
{
    public LangtFunction(LangtFunctionGroup group) : base(group.HoldingScope)
    {
        Group = group;
    }

    public LangtFunctionGroup Group {get;}

    public required LangtFunctionType Type {get; init;}
    public required string[] ParameterNames {get; init;}
    public required bool IsExtern {get; init;}

    public override string Name => Group.Name;
    public override string DisplayName => Group.DisplayName;
    public override string FullName => Group.FullName;
}