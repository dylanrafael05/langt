using System.Diagnostics.CodeAnalysis;
using Langt.AST;
using Langt.Message;
using Langt.Structure;
using Langt.Structure.Collections;

using Results.Interfaces;

namespace Langt.Utility;

public record StaticReference(SourceRange Range, IResolutionlike Item, bool IsDefinition = false) : IComparable<StaticReference>
{
    public int CompareTo(StaticReference? other)
        => other is null 
            ? 1 
            : Range.CharStart.CompareTo(other.Range.CharStart);
}

public record BindingOptions(bool TargetTypeDependent, bool ResolutionNotFound, OrderedList<StaticReference> References) : IResultMonoid<BindingOptions>
{
    public static BindingOptions Identity => new(false, false, new());

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
            TargetTypeDependent || opt.TargetTypeDependent,
            ResolutionNotFound  || opt.ResolutionNotFound,
            References      .Merge(opt.References)
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
    public static Result<T> DiagError<T>(MsgInfo info, SourceRange range)
        => Result.Error<T>(Diagnostic.Error(info, range));
    public static Result<T> AppendWarning<T>(this Result<T> w, MsgInfo info, SourceRange range)
        => w.WithMetadata(Diagnostic.Warning(info, range));
    public static Result<T> AppendNote<T>(this Result<T> w, MsgInfo info, SourceRange range)
        => w.WithMetadata(Diagnostic.Note(info, range));

    public static BindingOptions GetBindingOptions(this IResultlike r)
        => r.GetSingleton<BindingOptions>();
    public static bool IsTargetTypeDependent(this IResultlike r)
        => r.GetBindingOptions().TargetTypeDependent;
    public static bool IsResolutionNotFound(this IResultlike r)
        => r.GetBindingOptions().ResolutionNotFound;
    
    public static R ModifyBindingOptions<R>(this R r, Func<BindingOptions, BindingOptions> modifier) where R : IResultlike, IModdableResultlike<R>
        => r.ModifySingleton(modifier);
    public static R AsTargetTypeDependent<R>(this R r) where R : IResultlike, IModdableResultlike<R>
        => r.ModifyBindingOptions(b => b with {TargetTypeDependent = true});
    public static R AsResolutionNotFound<R>(this R r) where R : IResultlike, IModdableResultlike<R>
        => r.ModifyBindingOptions(b => b with {ResolutionNotFound = true});
    public static R AddStaticReference<R>(this R r, StaticReference reference) where R : IResultlike, IModdableResultlike<R>
        => r.ModifyBindingOptions(b => {b.Add(reference); return b;});
    public static R AddStaticReference<R>(this R r, SourceRange reference, IResolutionlike item, bool isDefinition = false) where R : IResultlike, IModdableResultlike<R>
        => r.AddStaticReference(new(reference, item, isDefinition));

    public static ResultBuilder WithDgnError(this ResultBuilder builder, MsgInfo info, SourceRange range)
        => builder.WithError(Diagnostic.Error(info, range));
    public static void AddDgnError(this ResultBuilder builder, MsgInfo info, SourceRange range)
        => builder.AddError(Diagnostic.Error(info, range));
    public static void AddWarning(this ResultBuilder builder, MsgInfo info, SourceRange range)
        => builder.AddMetadata(Diagnostic.Warning(info, range));
    public static void AddNote(this ResultBuilder builder, MsgInfo info, SourceRange range)
        => builder.AddMetadata(Diagnostic.Note(info, range));
}