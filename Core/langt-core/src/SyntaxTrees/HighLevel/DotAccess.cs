using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

public record DotAccess(ASTNode Left, ASTToken Dot, ASTToken Right) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Dot, Right};

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        // Get all input results
        var iptResult = Left.Bind(state, new TypeCheckOptions {AutoDeference = false});
        if(!iptResult) return iptResult;

        // Create output result builder from input
        var builder = ResultBuilder.Empty().WithData(iptResult);

        // Deconstruct and get values
        var left = iptResult.Value;
        var hasResolution = left.HasResolution;

        if(!left.Type.IsReference || left.Type.ElementType is not LangtStructureType structureType)
        {
            return builder.WithDgnError($"Cannot use a '.' access on a non-structure type", Range)
                .BuildError<BoundASTNode>();
        }

        if(!structureType.TryResolveField(Right.ContentStr, out var field, out var index))
        {
            return builder.WithDgnError($"Unknown field {Right.ContentStr} for type {structureType.Name}", Range)
                .BuildError<BoundASTNode>();
        }

        var result = new BoundStructFieldAccess(this, left)
        {
            Field = field!,
            FieldIndex = index
        };

        return builder.Build<BoundASTNode>(result);
    }
}
