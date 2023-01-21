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

    public IReadOnlyDictionary<string, IResolution> NamedItems => innerScope.NamedItems;

    public Result<TOut> Resolve<TOut>(string input, string outputType, SourceRange range, bool propogate = false) where TOut : INamed
        => innerScope.Resolve<TOut>(input, outputType, range, propogate);
    public Result<TIn> Define<TIn>(Func<IScope, TIn> constructor, SourceRange sourceRange) where TIn : IResolution
        => innerScope.Define(_ => constructor(this), sourceRange);
}
