using System.Diagnostics.CodeAnalysis;
using Langt.Structure;
using Langt.Structure.Resolutions;
using Langt.Utility;

namespace Langt.AST;

// TODO: is it safe to allow for "no fail" execution of passes?
// TODO: in what circumstances would "no fail" execution be requried?
// It appears that the main case for this is resolution; is there another way to report resolution failures?

public record ASTPassState(Context CTX, bool Noisy, bool CanFail);

public record GeneralPassState(Context CTX, bool Noisy, bool CanFail) : ASTPassState(CTX, Noisy, CanFail)
{
    public static GeneralPassState Start(Context gen) => new(gen, true, true);
}

public struct TypeCheckOptions
{
    public LangtType? TargetType {get; init;} = null;
    public bool AutoDeference {get; init;} = true;
    public IScope? PredefinedBlockScope {get; init;} = null;
    public bool AllowNamespaceDefinitions {get; init;} = false;

    public bool HasPredefinedBlockScope => PredefinedBlockScope is not null;

    public TypeCheckOptions()
    {}
}
