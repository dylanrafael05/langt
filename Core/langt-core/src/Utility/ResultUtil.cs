using System.Diagnostics.CodeAnalysis;
using Langt.AST;
using Langt.Structure;
using Langt.Structure.Collections;
using Langt.Structure.Resolutions;
using Results.Interfaces;

namespace Langt.Utility;

public record StaticReference(SourceRange Range, IResolution Item, bool IsDefinition = false) : IComparable<StaticReference>
{
    public int CompareTo(StaticReference? other)
        => other is null 
            ? 1 
            : Range.CharStart.CompareTo(other.Range.CharStart);
}

public record BindingOptions(bool TargetTypeDependent, OrderedList<StaticReference> References) : IResultMonoid<BindingOptions>
{
    public static BindingOptions Identity => new(false, new());

    public void Add(StaticReference r) 
    {
        References.Add(r);
    }

    public bool TryFold(IResultMonoid other, [NotNullWhen(true)] out IResultMonoid? result)
    {
        result = null;

        if(other is not BindingOptions opt) return false;

        result = new BindingOptions
        (
            this.TargetTypeDependent || opt.TargetTypeDependent,
            this.References             .Merge(opt.References)
        );

        return true;
    }
}

public struct SilentError : IResultError
{
    public IResultMetadata? TryDemote() => null;
}

public static class ResultUtil
{
    public static Result<T> DiagError<T>(string message, SourceRange range)
        => Result.Error<T>(Diagnostic.Error(message, range));
    public static Result<T> AppendWarning<T>(this Result<T> w, string message, SourceRange range)
        => w.WithMetadata(Diagnostic.Warning(message, range));
    public static Result<T> AppendNote<T>(this Result<T> w, string message, SourceRange range)
        => w.WithMetadata(Diagnostic.Note(message, range));

    public static BindingOptions GetBindingOptions(this IResultlike r)
        => r.GetSingleton<BindingOptions>();
    public static R ModifyBindingOptions<R>(this R r, Func<BindingOptions, BindingOptions> modifier) where R : IResultlike, IModdable<R>
        => r.ModifySingleton(modifier);
    public static R AsTargetTypeDependent<R>(this R r) where R : IResultlike, IModdable<R>
        => r.ModifyBindingOptions(b => b with {TargetTypeDependent = true});
    public static R AddStaticReference<R>(this R r, StaticReference reference) where R : IResultlike, IModdable<R>
        => r.ModifyBindingOptions(b => {b.Add(reference); return b;});
    public static R AddStaticReference<R>(this R r, SourceRange reference, IResolution item, bool isDefinition = false) where R : IResultlike, IModdable<R>
        => r.AddStaticReference(new(reference, item, isDefinition));

    public static ResultBuilder WithDgnError(this ResultBuilder builder, string message, SourceRange range)
        => builder.WithError(Diagnostic.Error(message, range));
    public static void AddDgnError(this ResultBuilder builder, string message, SourceRange range)
        => builder.AddError(Diagnostic.Error(message, range));
    public static void AddWarning(this ResultBuilder builder, string message, SourceRange range)
        => builder.AddMetadata(Diagnostic.Warning(message, range));
    public static void AddNote(this ResultBuilder builder, string message, SourceRange range)
        => builder.AddMetadata(Diagnostic.Note(message, range));
}