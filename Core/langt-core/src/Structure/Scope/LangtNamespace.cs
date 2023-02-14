using Langt.AST;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

// TODO: handle this! both a scope and resolution
public class LangtNamespace : Resolution, IScope
{
    private LangtScope innerScope;

    public override string Name {get;}

    public LangtNamespace(IScope scope, string name) : base(scope) 
    {
        innerScope = new(scope);
        Name = name;
    }

    public IReadOnlyDictionary<string, IResolutionProducer> Items => innerScope.Items;

    public Result<TOut>? ResolveSelfPossibly<TOut>(string input, string outputType, ASTPassState state, SourceRange range, TypeCheckOptions? optionsMaybe = null) where TOut : INamed
    {
        return ((IScope)innerScope).ResolveSelfPossibly<TOut>(input, outputType, state, range, optionsMaybe);
    }
    public Result<TOut> Resolve<TOut>(string input, string outputType, ASTPassState state, SourceRange range, TypeCheckOptions? optionsMaybe = null) where TOut : INamed
    {
        return ((IScope)innerScope).Resolve<TOut>(input, outputType, state, range, optionsMaybe);
    }
    public Result<TIn> Define<TIn>(Func<IScope, TIn> constructor, SourceRange sourceRange) where TIn : IResolutionProducer
    {
        return ((IScope)innerScope).Define(constructor, sourceRange);
    }
}
