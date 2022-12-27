using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

public record DotAccess(ASTNode Left, ASTToken Dot, ASTToken Right) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Dot, Right};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString(". access");
        visitor.Visit(Left);
        visitor.Visit(Right);
    }


    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        // Get all input results
        var iptResult = Left.Bind(state, new TypeCheckOptions {AutoDeferenceLValue = false});
        if(!iptResult) return iptResult;

        // Create output result builder from input
        var builder = ResultBuilder.Empty().WithData(iptResult);

        // Deconstruct and get values
        var left = iptResult.Value;

        var result = new BoundDotAccess(this, left)
        {
            HasResolution = left.HasResolution //&& left.Resolution is not LangtVariable //TODO: ?
        };

        if (result.HasResolution)
        {
            if(left.Resolution is not LangtNamespace ns) 
            {
                return builder.WithDgnError($"Cannot access a static member of something that is not a namespace", Range)
                    .Build<BoundASTNode>();
            }

            var resolutionResult = ns.Resolve(Right.ContentStr, Range);
            builder.AddData(resolutionResult);
            
            result.Resolution = resolutionResult.OrDefault();
        }

        if(!left.TransformedType.IsPointer || left.TransformedType.PointeeType is not LangtStructureType structureType)
        {
            return builder.WithDgnError($"Cannot use a '.' access on a non-structure type", Range)
                .Build<BoundASTNode>();

            // TODO: modify this to include namespace getters! (how will that work?)
        }

        if(!structureType.TryResolveField(Right.ContentStr, out var field, out var index, out _))
        {
            return builder.WithDgnError($"Unknown field {Right.ContentStr} for type {structureType.Name}", Range)
                .Build<BoundASTNode>();
        }

        result.Field = field;
        result.FieldIndex = index;

        result.RawExpressionType = LangtType.PointerTo(result.Field!.Type);

        return builder.Build<BoundASTNode>(result);
    }
}
