using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

/// <summary>
/// Represents a compiler error which moves silently up the syntax tree during passes
/// because it has already been reported.
/// </summary>
public class SilentError : IResultError
{
    public IResultMetadata? TryDemote() => null;
    private SilentError() {}

    public static SilentError Create() => new();
}

/// <summary>
/// A node which represents an invalid syntax item, and which fails with 'SilentError' whenever
/// passes touch it.
/// </summary>
public record ASTInvalid(SourceRange ErrRange) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new();
    public override SourceRange Range => ErrRange;

    public override Result HandleDefinitions(Context ctx)
        => Result.Error(SilentError.Create());
    public override Result RefineDefinitions(Context ctx)
        => Result.Error(SilentError.Create());
    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
        => Result.Error<BoundASTNode>(SilentError.Create());
}
