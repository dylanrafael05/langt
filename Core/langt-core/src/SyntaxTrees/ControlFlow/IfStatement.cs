using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;
using Langt.Structure;

namespace Langt.AST;

public record BoundIfStatement(IfStatement Source, BoundASTNode Condition, BoundASTNode Block, BoundASTNode? Else) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Condition, Block};

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(lowerer.CurrentFunction is null) 
        {
            lowerer.Project.Logger.Fatal("Cannot lower if statement when not part of a function!");
            throw new Exception();
        }

        var trueBB  = lowerer.Context.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, Source.If.Range.CharStart+".if.ontrue");
        var falseBB = lowerer.Context.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, Source.If.Range.CharStart+".if.else"  ); 
        var endBB   = lowerer.Context.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, Source.If.Range.CharStart+".if.end"   );

        Condition.Lower(lowerer);
        var c = lowerer.PopValue(DebugSourceName);

        lowerer.Builder.BuildCondBr(c.LLVM, trueBB, falseBB);

        lowerer.Builder.PositionAtEnd(trueBB);
        lowerer.OpenScope();
            Block.Lower(lowerer);
            if(!Block.Returns) lowerer.Builder.BuildBr(endBB);
        lowerer.CloseScope();

        lowerer.Builder.PositionAtEnd(falseBB);
        if(Else is not null)
        {
            lowerer.OpenScope();
                Else.Lower(lowerer);
            lowerer.CloseScope();
        }
        if(!(Else?.Returns ?? false)) lowerer.Builder.BuildBr(endBB);

        lowerer.Builder.PositionAtEnd(endBB);
    }
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