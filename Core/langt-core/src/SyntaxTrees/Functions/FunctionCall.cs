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

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        FunctionAST.TypeCheck(state);
        
        var givenArgs = Arguments.Values.ToArray();
        
        foreach(var arg in givenArgs)
        {
            arg.TypeCheck(state);
        }

        if(FunctionAST.HasResolution && FunctionAST.Resolution is LangtFunctionGroup functionGroup)
        {
            var function = functionGroup.MatchOverload(givenArgs, this, state);

            if(function is null) return;

            funcType = function.Type;
            RawExpressionType = funcType.ReturnType;

            hasDirectFunction = true;
            functionValue = function.LLVMFunction;
        }
        else if(FunctionAST.TransformedType.IsFunctionPtr)
        {
            hasDirectFunction = false;

            funcType = (LangtFunctionType)FunctionAST.TransformedType.PointeeType!;
            RawExpressionType = funcType!.ReturnType;

            if(!funcType.MakeSignatureMatch(givenArgs, state))
            {
                state.Error(
                    $"Could not call a function pointer of type {funcType.Name} " +
                    $"with arguments of type {string.Join(", ", Arguments.Values.Select(a => a.TransformedType.GetFullName()))}",
                    Range
                );
            }
        }
        else 
        {
            state.Error("Cannot call a non-functional expression", Range);
        }
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        FunctionAST.Lower(lowerer);
        if(!hasDirectFunction)
        {
            functionValue = lowerer.PopValue(DebugSourceName).LLVM;
        }

        var args = Arguments.Values.ToArray();
        var llvmArgs = new LLVMValueRef[args.Length];

        for(var i = 0; i < args.Length; i++)
        {
            args[i].Lower(lowerer);
            llvmArgs[i] = lowerer.PopValue(DebugSourceName).LLVM;
        }

        var r = lowerer.Builder.BuildCall2(
            lowerer.LowerType(funcType!),
            functionValue, 
            llvmArgs
        );

        if(funcType!.ReturnType != LangtType.None)
        {
            lowerer.PushValue(funcType!.ReturnType, r, DebugSourceName);
        }
    }
}