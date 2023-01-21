using Langt.Structure;
using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundWhileStatement(WhileStatement Source, BoundASTNode Condition, BoundASTNode Block) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Condition, Block};
}

public record WhileStatement(ASTToken While, ASTNode Condition, Block Block) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {While, Condition, Block};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("While");
        visitor.Visit(Condition);
        visitor.PutString("... then ...");
        visitor.Visit(Block);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var results = Result.All
        (
            Condition.BindMatchingExprType(state, LangtType.Bool),
            Block.Bind(state)
        );

        if(!results) return results.ErrorCast<BoundASTNode>();

        var (cond, blk) = results.Value;
        
        return ResultBuilder.From(results).Build<BoundASTNode>
        (
            new BoundWhileStatement(this, cond, blk)
        );
    }
}
