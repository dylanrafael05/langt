using System.Diagnostics.CodeAnalysis;
using Langt.Structure;
using Langt.Utility;

namespace Langt.AST;

// TODO: is it safe to allow for "no fail" execution of passes?
// TODO: in what circumstances would "no fail" execution be requried?
// It appears that the main case for this is resolution; is there another way to report resolution failures?

public record ASTPassState(Context CG, bool Noisy, bool CanFail);

public record GeneralPassState(Context CG, bool Noisy, bool CanFail) : ASTPassState(CG, Noisy, CanFail)
{
    public static GeneralPassState Start(Context gen) => new(gen, true, true);
}

public struct TypeCheckOptions
{
    public LangtType? TargetType {get; init;} = null;
    public bool AutoDeferenceLValue {get; init;} = true;
    public IScope? PredefinedBlockScope {get; init;} = null;
    public bool AllowNamespaceDefinitions {get; init;} = false;

    public bool HasPredefinedBlockScope => PredefinedBlockScope is not null;

    public TypeCheckOptions()
    {}
}
