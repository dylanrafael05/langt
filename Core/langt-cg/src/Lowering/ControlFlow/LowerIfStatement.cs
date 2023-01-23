using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerIfStatement : ILowerImplementation<BoundIfStatement>
{
    public void LowerImpl(CodeGenerator cg, BoundIfStatement node)
    {
        if(cg.CurrentFunction is {Handle : 0}) 
        {
            cg.Logger.Fatal("Cannot lower if statement when not part of a function!");
            throw new Exception();
        }

        var trueBB  = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.If.Range.CharStart+".if.ontrue");
        var falseBB = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.If.Range.CharStart+".if.else"  ); 
        var endBB   = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.If.Range.CharStart+".if.end"   );

        cg.Lower(node.Condition);
        var c = cg.PopValue(node.DebugSourceName);

        cg.Builder.BuildCondBr(c.LLVM, trueBB, falseBB);

        cg.Builder.PositionAtEnd(trueBB);
            cg.Lower(node.Block);
            if(!node.Block.Returns) cg.Builder.BuildBr(endBB);

        cg.Builder.PositionAtEnd(falseBB);

        if(node.Else is not null)
        {
            cg.Lower(node.Else);
            if(!node.Else.Returns) cg.Builder.BuildBr(endBB);
        }

        cg.Builder.PositionAtEnd(endBB);
    }
}
