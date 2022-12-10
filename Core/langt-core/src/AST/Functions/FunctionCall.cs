using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record FunctionCall(ASTNode FunctionAST, ASTToken Open, SeparatedCollection<ASTNode> Arguments, ASTToken End) : ASTNode, IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {FunctionAST, Open, Arguments, End};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Function Call");
        visitor.Visit(FunctionAST);
        foreach(var arg in Arguments.Values)
        {
            visitor.Visit(arg);
        }
    }

    public LangtFunction? Function {get; private set;}
    private LangtFunctionType? funcType;

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        FunctionAST.TypeCheck(generator);

        if(!FunctionAST.HasResolution || FunctionAST.Resolution is not LangtFunctionGroup functionGroup)
        {
            generator.Diagnostics.Error("Cannot call a non-functional expression", Range);
            return;
        }
        
        var givenArgs = Arguments.Values.ToArray();
        foreach(var arg in givenArgs)
        {
            arg.TypeCheck(generator);
        }

        Function = functionGroup.MatchOverload(givenArgs, Range, generator);

        if(Function is null) return;

        funcType = Function.Type;
        ExpressionType = funcType.ReturnType;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        var args = Arguments.Values.ToArray();
        var llvmArgs = new LLVMValueRef[args.Length];

        for(var i = 0; i < args.Length; i++)
        {
            args[i].Lower(lowerer);
            llvmArgs[i] = lowerer.PopValue().LLVM;
        }

        lowerer.PushValue(
            funcType!.ReturnType,
            lowerer.Builder.BuildCall2(lowerer.LowerType(Function!.Type), Function!.LLVMFunction, llvmArgs)
        );
    }
}