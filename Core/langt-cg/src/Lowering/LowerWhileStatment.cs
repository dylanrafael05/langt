using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerWhileStatment : ILowerImplementation<BoundWhileStatement>
{
    public void LowerImpl(CodeGenerator cg, BoundWhileStatement node)
    {
        if(cg.CurrentFunction is null) 
        {
            cg.Logger.Fatal("Cannot lower while statement when not part of a function!");
            throw new Exception();
        }

        var condBB  = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!.LLVMFunction, node.Source.While.Range.CharStart+".while.cond" );
        var trueBB  = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!.LLVMFunction, node.Source.While.Range.CharStart+".while.true" ); 
        var breakBB = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!.LLVMFunction, node.Source.While.Range.CharStart+".while.break");

        cg.Builder.BuildBr(condBB);
        cg.Builder.PositionAtEnd(condBB);

        cg.Lower(node.Condition);
        var c = cg.PopValue(node.DebugSourceName);

        cg.Builder.BuildCondBr(c.LLVM, trueBB, breakBB);

        cg.Builder.PositionAtEnd(trueBB);
            cg.Lower(node.Block);
            cg.Builder.BuildBr(condBB);

        cg.Builder.PositionAtEnd(breakBB);
    }
}