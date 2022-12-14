using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;
using Langt.Structure;

namespace Langt.AST;

public record IfStatement(ASTToken If, ASTNode Condition, Block Block, ElseStatement? Else) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {If, Condition, Block, Else};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("If");
        visitor.Visit(Condition);
        visitor.PutString("... then ...");
        visitor.Visit(Block);
        if(Else is not null) visitor.Visit(Else);
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Condition.TypeCheck(generator);
        Block.TypeCheck(generator);

        if(!generator.MakeMatch(Codegen.LangtType.Bool, Condition))
        {
            generator.Project.Diagnostics.Error("If condition must be a boolean, but was instead " + Condition.TransformedType.Name, Condition.Range);
        }

        if(Else is not null)
        {
            Else.TypeCheck(generator);

            Returns = Block.Returns && Else.Returns;
        }
        
        ExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(lowerer.CurrentFunction is null) 
        {
            lowerer.Project.Logger.Fatal("Cannot lower if statement when not part of a function!");
            throw new Exception();
        }

        var trueBB  = lowerer.LLVMContext.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, If.Range.CharStart+".if.ontrue");
        var falseBB = lowerer.LLVMContext.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, If.Range.CharStart+".if.else"  ); 
        var endBB   = lowerer.LLVMContext.AppendBasicBlock(lowerer.CurrentFunction!.LLVMFunction, If.Range.CharStart+".if.end"   );

        Condition.Lower(lowerer);
        var c = lowerer.PopValue();

        lowerer.Builder.BuildCondBr(c.LLVM, trueBB, falseBB);

        lowerer.Builder.PositionAtEnd(trueBB);
        lowerer.CreateUnnamedScope();
            Block.Lower(lowerer);
            if(!Block.Returns) lowerer.Builder.BuildBr(endBB);
        lowerer.CloseScope();

        lowerer.Builder.PositionAtEnd(falseBB);
        if(Else is not null)
        {
            lowerer.CreateUnnamedScope();
                Else.Lower(lowerer);
            lowerer.CloseScope();
        }
        if(!(Else?.Returns ?? false)) lowerer.Builder.BuildBr(endBB);

        lowerer.Builder.PositionAtEnd(endBB);
    }
}