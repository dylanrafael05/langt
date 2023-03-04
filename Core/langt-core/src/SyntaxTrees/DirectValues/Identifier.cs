using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record Identifier(ASTToken Tok) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Tok};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        var resolution = ctx.ResolutionScope.ResolveDirect(Tok.ContentStr, Range, ctx);
        builder.AddData(resolution);
        if(!resolution) return builder.BuildError<BoundASTNode>();

        builder.AddStaticReference(Range, resolution.Value);

        return builder.Build<BoundASTNode>
        (
            new BoundEmpty(this)
            {
                HasResolution = true,
                Resolution = resolution.Value
            }
        );
    }
}
