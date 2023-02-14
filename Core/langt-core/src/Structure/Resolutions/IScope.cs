using Langt.AST;

namespace Langt.Structure.Resolutions;

public interface IScope 
{
    IScope? HoldingScope {get;}
    IReadOnlyDictionary<string, IWeakRes> Items {get;}

    Result<WeakRes<TOut>> ResolveSelf<TOut>(string input, string outputType, SourceRange range) where TOut : INamed;
    Result<WeakRes<TOut>> Resolve<TOut>(string input, string outputType, SourceRange range) where TOut : INamed;
    Result<WeakRes<TIn>> Define<TIn>(Func<IScope, WeakRes<TIn>> constructor, SourceRange sourceRange) where TIn : INamed;
}
