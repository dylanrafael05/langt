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

    //public LangtFunction? Function {get; private set;}
    private bool hasDirectFunction;
    private LLVMValueRef functionValue;
    private LangtFunctionType? funcType;

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        FunctionAST.TypeCheck(generator);
        
        var givenArgs = Arguments.Values.ToArray();
        
        foreach(var arg in givenArgs)
        {
            arg.TypeCheck(generator);
        }

        if(FunctionAST.HasResolution && FunctionAST.Resolution is LangtFunctionGroup functionGroup)
        {
            var function = functionGroup.MatchOverload(givenArgs, Range, generator);

            if(function is null) return;

            funcType = function.Type;
            ExpressionType = funcType.ReturnType;

            hasDirectFunction = true;
            functionValue = function.LLVMFunction;
        }
        else if(FunctionAST.TransformedType.IsFunctionPtr)
        {
            hasDirectFunction = false;

            funcType = (LangtFunctionType)FunctionAST.TransformedType.PointeeType!;
            ExpressionType = funcType!.ReturnType;

            if(!funcType.MakeSignatureMatch(Arguments.Values.ToArray(), generator))
            {
                generator.Diagnostics.Error(
                    $"Could not call a function pointer of type {funcType.Name} " +
                    $"with arguments of type {string.Join(", ", Arguments.Values.Select(a => a.TransformedType.GetFullName()))}", 
                    Range
                );
            }
        }
        else 
        {
            generator.Diagnostics.Error("Cannot call a non-functional expression", Range);
        }
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        FunctionAST.Lower(lowerer);
        if(!hasDirectFunction)
        {
            functionValue = lowerer.PopValue().LLVM;
        }

        var args = Arguments.Values.ToArray();
        var llvmArgs = new LLVMValueRef[args.Length];

        for(var i = 0; i < args.Length; i++)
        {
            args[i].Lower(lowerer);
            llvmArgs[i] = lowerer.PopValue().LLVM;
        }

        lowerer.PushValue(
            funcType!.ReturnType,
            lowerer.Builder.BuildCall2(
                lowerer.LowerType(funcType),
                functionValue, 
                llvmArgs
            )
        );
    }
}