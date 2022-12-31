using Langt.AST;

namespace Langt.Utility;

public class TargetTypeDependentTag : IResultMetadata
{}

public class SilentError : IResultError
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
    
    public static Result<BoundASTNode> ClearTargetTypeDependent(this Result<BoundASTNode> r) 
        => r.ExcludingMetadata(r.Metadata.OfType<TargetTypeDependentTag>());
    public static Result<BoundASTNode> TagTargetTypeDependent(this Result<BoundASTNode> r) 
        => r.WithMetadata(new TargetTypeDependentTag());
    public static bool IsTargetTypeDependent(this IResultlike r) 
        => r.AnyMeta<TargetTypeDependentTag>();

    public static ResultBuilder WithDgnError(this ResultBuilder builder, string message, SourceRange range)
        => builder.WithError(Diagnostic.Error(message, range));
    public static void AddDgnError(this ResultBuilder builder, string message, SourceRange range)
        => builder.AddError(Diagnostic.Error(message, range));
    public static void AddWarning(this ResultBuilder builder, string message, SourceRange range)
        => builder.AddMetadata(Diagnostic.Warning(message, range));
    public static void AddNote(this ResultBuilder builder, string message, SourceRange range)
        => builder.AddMetadata(Diagnostic.Note(message, range));
}