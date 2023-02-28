using System.Diagnostics.CodeAnalysis;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtFunction : IFullNamed
{
    public LangtFunction(LangtFunctionGroup group)
    {
        Group = group;
    }

    public LangtFunctionGroup Group {get;}

    public required LangtFunctionType Type {get; init;}
    public required string[] ParameterNames {get; init;}
    public required bool IsExtern {get; init;}

    public string Name => Group.Name;
    public string DisplayName => Group.DisplayName;
    public string FullName => Group.FullNameSimple();
}