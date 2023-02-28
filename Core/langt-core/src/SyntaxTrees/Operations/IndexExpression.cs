using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundIndexExpression(IndexExpression Source, BoundASTNode Value, BoundASTNode Index) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value, Index};
    public override bool IsAssignable => true;
}

public record IndexExpression(ASTNode Value, ASTToken Open, SeparatedCollection<ASTNode> Arguments, ASTToken Close) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Value, Open, Arguments, Close};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        var op = ctx.GetGlobalFunction(LangtWords.MagicIndex);
        var args = Arguments.Values.Prepend(Value).ToArray();

        var fr = op.ResolveOverload(args, Range, ctx);
        builder.AddData(fr);

        if(!builder) return builder.BuildError<BoundASTNode>();

        var fn = fr.Value;

        return builder.Build<BoundASTNode>
        (
            new BoundFunctionCall(this, fn.Function, fn.OutputParameters.Value.ToArray())
        );
    }
}
