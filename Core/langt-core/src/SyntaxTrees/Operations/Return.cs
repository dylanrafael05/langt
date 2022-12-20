using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record Return(ASTToken ReturnTok, ASTNode? Value = null) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {ReturnTok, Value};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("return");
        if(Value is not null)
        {
            visitor.Visit(Value);
        }
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        if(Value is null) return;

        Value.TypeCheck(state);

        if(!state.MakeMatch(state.CG.CurrentFunction!.Type.ReturnType, Value))
        {
            state.Error($"Return type {Value.TransformedType.Name} does not match function return type {state.CG.CurrentFunction!.Type.ReturnType.Name}", Range);
        }

        Returns = true;
        
        RawExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(Value is null)
        {
            lowerer.Builder.BuildRetVoid();
        }
        else
        {
            Value.Lower(lowerer);
            lowerer.Builder.BuildRet(lowerer.PopValue(DebugSourceName).LLVM);
        }
    }
}