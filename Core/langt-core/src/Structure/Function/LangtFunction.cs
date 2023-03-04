using System.Diagnostics.CodeAnalysis;


namespace Langt.Structure;

public class LangtFunction : IResolutionlike
{
    public LangtFunction(LangtFunctionGroup group)
    {
        Group = group;
    }

    public LangtFunctionGroup Group {get;}

    public required LangtFunctionType Type {get; init;}
    public required string[] ParameterNames {get; init;}
    public required bool IsExtern {get; init;}
    [NotNull] public required string? Documentation {get; init;}
    [NotNull] public required SourceRange? DefinitionRange {get; init;}

    public string Name => Group.Name;
    public string DisplayName => Group.DisplayName;
    public string FullName => Group.FullNameSimple();

    IScope IResolutionlike.HoldingScope => Group.HoldingScope;
}