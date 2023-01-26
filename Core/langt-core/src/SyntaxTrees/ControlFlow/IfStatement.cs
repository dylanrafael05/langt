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

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("If");
        visitor.Visit(Condition);
        visitor.PutString("... then ...");
        visitor.Visit(Block);
        if(Else is not null) visitor.Visit(Else);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var results = Result.All
        (
            Condition.BindMatchingExprType(state, LangtType.Bool),
            Block.Bind(state)
        );
        var builder = ResultBuilder.From(results);

        if(!results) return builder.BuildError<BoundASTNode>();

        var (cond, block) = results.Value;

        BoundASTNode? boundElse = null;

        if(Else is not null)
        {
            var e = Else.Bind(state);
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