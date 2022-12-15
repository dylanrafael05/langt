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

    public override void TypeCheckSelf(CodeGenerator generator)
    {
        if(Value is null) return;

        Value.TypeCheck(generator);

        if(!generator.MakeMatch(generator.CurrentFunction!.Type.ReturnType, Value))
        {
            generator.Diagnostics.Error($"Return type {Value.TransformedType.Name} does not match function return type {generator.CurrentFunction!.Type.ReturnType.Name}", Range);
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