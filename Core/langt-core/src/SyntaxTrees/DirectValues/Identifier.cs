using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record Identifier(ASTToken Tok) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Tok};

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        var resolution = state.CTX.ResolutionScope.Resolve(Tok.ContentStr, Range);
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
