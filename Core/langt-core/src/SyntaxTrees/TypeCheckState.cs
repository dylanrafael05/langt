using System.Diagnostics.CodeAnalysis;
using Langt.Structure;

using Langt.Utility;

namespace Langt.AST;

public readonly struct TypeCheckOptions
{
    public LangtType? TargetType {get; init;} = null;
    public bool AutoDeference {get; init;} = true;
    public IScope? PredefinedBlockScope {get; init;} = null;
    public bool AllowNamespaceDefinitions {get; init;} = false;

    public bool HasPredefinedBlockScope => PredefinedBlockScope is not null;

    public TypeCheckOptions()
    {}
}
