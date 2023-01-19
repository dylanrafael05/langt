using Langt.Codegen;
using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundWhileStatement(WhileStatement Source, BoundASTNode Condition, BoundASTNode Block) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Condition, Block};

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(lowerer.CurrentFunction is null) 
        {
            lowerer.Project.Logger.Fatal("Cannot lower while statement when not part of a function!");
            throw new Exception();
        }

        var condBB  = lowerer.Context.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, Source.While.Range.CharStart+".while.cond" );
        var trueBB  = lowerer.Context.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, Source.While.Range.CharStart+".while.true" ); 
        var breakBB = lowerer.Context.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, Source.While.Range.CharStart+".while.break");

        lowerer.Builder.BuildBr(condBB);
        lowerer.Builder.PositionAtEnd(condBB);

        Condition.Lower(lowerer);
        var c = lowerer.PopValue(DebugSourceName);

        lowerer.Builder.BuildCondBr(c.LLVM, trueBB, breakBB);

        lowerer.Builder.PositionAtEnd(trueBB);
        lowerer.OpenScope(); // TODO: this does nothing! scopes should be applied only during the binding phase
            Block.Lower(lowerer);
            lowerer.Builder.BuildBr(condBB);
        lowerer.CloseScope();

        lowerer.Builder.PositionAtEnd(breakBB);
    }
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
