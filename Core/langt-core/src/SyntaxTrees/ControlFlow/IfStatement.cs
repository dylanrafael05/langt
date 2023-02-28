using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundIfStatement(IfStatement Source, BoundASTNode Condition, BoundASTNode Block, BoundASTNode? Else) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Condition, Block};
}

public record IfStatement(ASTToken If, ASTNode Condition, Block Block, ElseStatement? Else) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {If, Condition, Block, Else};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        var results = Result.All
        (
            Condition.BindMatchingExprType(ctx, LangtType.Bool),
            Block.Bind(ctx)
        );
        var builder = ResultBuilder.From(results);

        if(!results) return builder.BuildError<BoundASTNode>();

        var (cond, block) = results.Value;

        BoundASTNode? boundElse = null;

        if(Else is not null)
        {
            var e = Else.Bind(ctx);
            builder.AddData(e);

            if(!e) return builder.BuildError<BoundASTNode>();

            boundElse = e.Value;
        }
        
        return builder.Build<BoundASTNode>
        (
            new BoundIfStatement(this, cond, block, boundElse)
            {
                Type = LangtType.None,
                Returns = block.Returns && (boundElse?.Returns ?? false)
            }
        );
    }
}