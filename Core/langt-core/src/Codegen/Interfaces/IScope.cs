namespace Langt.Codegen;

public interface IScope 
{
    IScope? HoldingScope {get;}
    IReadOnlyDictionary<string, IResolution> NamedItems {get;}

    Result<TOut> Resolve<TOut>(string input, string outputType, SourceRange range, bool propogate = false) where TOut : INamed;
    Result<TIn> Define<TIn>(Func<IScope, TIn> constructor, SourceRange sourceRange) where TIn : IResolution;
}
