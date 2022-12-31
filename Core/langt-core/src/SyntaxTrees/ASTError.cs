using Langt.Codegen;
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

    public override void Dump(VisitDumper visitor)
    {}

    public override Result HandleDefinitions(ASTPassState state)
        => Result.Error(SilentError.Create());
    public override Result RefineDefinitions(ASTPassState state)
        => Result.Error(SilentError.Create());
    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Result.Error<BoundASTNode>(SilentError.Create());
}
