using Langt.Codegen;
using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record WhileStatement(ASTToken While, ASTNode Condition, Block Block) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {While, Condition, Block};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("While");
        visitor.Visit(Condition);
        visitor.PutString("... then ...");
        visitor.Visit(Block);
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Condition.TypeCheck(generator);
        Block.TypeCheck(generator);

        if(!generator.MakeMatch(LangtType.Bool, Condition))
        {
            generator.Project.Diagnostics.Error("While condition must be a boolean, but was instead " + Condition.TransformedType.Name, Condition.Range);
        }
        
        ExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(lowerer.CurrentFunction is null) 
        {
            lowerer.Project.Logger.Fatal("Cannot lower while statement when not part of a function!");
            throw new Exception();
        }

        var condBB  = lowerer.LLVMContext.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, While.Range.CharStart+".while.cond" );
        var trueBB  = lowerer.LLVMContext.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, While.Range.CharStart+".while.true" ); 
        var breakBB = lowerer.LLVMContext.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, While.Range.CharStart+".while.break");

        lowerer.Builder.BuildBr(condBB);
        lowerer.Builder.PositionAtEnd(condBB);

        Condition.Lower(lowerer);
        var c = lowerer.PopValue();

        lowerer.Builder.BuildCondBr(c.LLVM, trueBB, breakBB);

        lowerer.Builder.PositionAtEnd(trueBB);
        lowerer.CreateUnnamedScope();
            Block.Lower(lowerer);
            lowerer.Builder.BuildBr(condBB);
        lowerer.CloseScope();

        lowerer.Builder.PositionAtEnd(breakBB);

        // TODO: CREATE NEW VISITORS FOR EACH SUB-TASK (typing, lowering, definitions), 
        // TODO: CHANGE CodeGenerator to be Lowerer and Context
        // TODO: COMPLETE TypeChecking system and Conversion system in order to localize all errors to inside this program (no llvm errors)
        // TODO: Test all of this mightily when finished

        //! PLEASE PUSH NEXT TIME YOU MAKE BRAKING CHANGES LIKE THIS YOU DUMBASS
    }
}
