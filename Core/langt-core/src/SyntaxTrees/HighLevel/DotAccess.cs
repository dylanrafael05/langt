using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;
using Langt.Utility;
using Langt.Message;

namespace Langt.AST;

public record DotAccess(ASTNode Left, ASTToken Dot, ASTToken Right) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Dot, Right};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        // Get all input results
        var iptResult = Left.Bind(ctx, new TypeCheckOptions {AutoDeference = false});
        if(!iptResult) return iptResult;

        // Create output result builder from input
        var builder = ResultBuilder.Empty().WithData(iptResult);

        // Deconstruct and get values
        var left = iptResult.Value;
        var hasResolution = left.HasResolution;

        if(!left.Type.IsStructure)
        {
            return builder.WithDgnError(Messages.Get("dot-not-struct", left.Type), Range)
                .BuildError<BoundASTNode>();
        }

        var structureType = left.Type.Structure;

        if(!structureType.ResolveField(Right.ContentStr, out var field))
        {
            return builder.WithDgnError(Messages.Get("dot-bad-field", left.Type, Right.ContentStr), Range)
                .BuildError<BoundASTNode>();
        }

        var result = new BoundStructFieldAccess(this, left)
        {
            Field = field
        };

        return builder.Build<BoundASTNode>(result);
    }
}
