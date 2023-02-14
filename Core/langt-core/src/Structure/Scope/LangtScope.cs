using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Langt.AST;
using Langt.Structure;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtScope : IScope
{
    public LangtScope(IScope? holdingScope)
    {
        HoldingScope = holdingScope;
    }
    
    public IScope? HoldingScope {get;}

    public virtual bool IsNamespace => false;
    public bool IsGlobalScope => HoldingScope == null;

    private readonly Dictionary<string, IWeakRes> items = new();
    public IReadOnlyDictionary<string, IWeakRes> Items => items;

    public virtual Result<WeakRes<TOut>> ResolveSelf<TOut>(
        string input,
        string outputType,
        SourceRange range) where TOut : INamed
    {
        // Check if the item exists in the named items stored by this scope
        if(items.TryGetValue(input, out var r))
        {
            if(r is WeakRes<TOut> weak)
                return Result.Success(weak);
        }

        return Result.Error<WeakRes<TOut>>(Diagnostic.Error($"Could not find {outputType} named {input}", range))
            .AsResolutionNotFound();
    }

    public virtual Result<WeakRes<TOut>> Resolve<TOut>(
        string input,
        string outputType,
        SourceRange range) where TOut: INamed
    {
        var builder = ResultBuilder.Empty();
        var result = ResolveSelf<TOut>(input, outputType, range);

        // Check the upper scope if it exists
        if(!result && HoldingScope is not null)
        {
            result = HoldingScope.Resolve<TOut>(input, outputType, range);
        }
        
        builder.AddData(result);

        // Return the result, null or not
        return builder.HasErrors ? builder.BuildError<WeakRes<TOut>>() : builder.Build(result!.Value);
    }

    public virtual Result<WeakRes<T>> Define<T>(Func<IScope, WeakRes<T>> constructor, SourceRange sourceRange) where T : INamed
    {
        var obj = constructor(this);

        ref var item = ref items.GetOrAddDefaultRef(obj.Name, out var exists);

        if(!exists)
        {
            return ResultBuilder
                .Empty()
                .WithDgnError($"Attempting to redefine name {obj.Name}", sourceRange)
                .BuildError<WeakRes<T>>();
        }

        item = obj;
        
        return Result.Success(obj);
    }
}