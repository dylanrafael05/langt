using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundFunctionCall(ASTNode Source, 
    BoundASTNode Function, 
    BoundASTNode[] Arguments, 
    bool HasDirectFunction,
    LLVMValueRef? FunctionValue,
    LangtFunctionType FunctionType
) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Function, Arguments};

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Function.Lower(lowerer);

        LLVMValueRef usedFnValue;

        if(!HasDirectFunction)
        {
            usedFnValue = lowerer.PopValue(DebugSourceName).LLVM;
        }
        else
        {
            usedFnValue = FunctionValue!.Value;
        }

        var llvmArgs = new LLVMValueRef[Arguments.Length];

        for(var i = 0; i < Arguments.Length; i++)
        {
            Arguments[i].Lower(lowerer);
            llvmArgs[i] = lowerer.PopValue(DebugSourceName).LLVM;
        }

        var r = lowerer.Builder.BuildCall2(
            lowerer.LowerType(FunctionType!),
            usedFnValue, 
            llvmArgs
        );

        if(FunctionType!.ReturnType != LangtType.None)
        {
            lowerer.PushValue(FunctionType!.ReturnType, r, DebugSourceName);
        }
    }
}

public record FunctionCall(ASTNode FunctionAST, ASTToken Open, SeparatedCollection<ASTNode> Arguments, ASTToken End) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {FunctionAST, Open, Arguments, End};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Function Call");
        visitor.Visit(FunctionAST);
        foreach(var arg in Arguments.Values)
        {
            visitor.Visit(arg);
        }
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        // Get all input results
        var iptResult = FunctionAST.Bind(state);
        if(!iptResult) return iptResult;

        // Create output result builder from input
        var builder = ResultBuilder.From(iptResult);

        // Deconstruct and get values
        var fn = iptResult.Value;
        var givenArgs = Arguments.Values.ToArray();

        // Create resultant variables
        LangtFunctionType funcType;
        bool hasDirectFunction;
        LLVMValueRef? directFunction = null;
        LangtType resultType;
        BoundASTNode?[] boundArgs;

        if(fn.HasResolution && fn.Resolution is LangtFunctionGroup functionGroup)
        {
            var resolveResult = functionGroup.ResolveOverload(givenArgs, this, state);
            builder.AddData(resolveResult);

            if(!resolveResult) return builder.Build<BoundASTNode>();

            var resolution = resolveResult.Value;

            funcType = resolution.Function.Type;
            resultType = funcType.ReturnType;

            hasDirectFunction = true;
            directFunction = resolution.Function.LLVMFunction;

            boundArgs = resolution.OutputParameters.Value.ToArray();
        }
        else if(fn.TransformedType.IsFunctionPtr)
        {
            hasDirectFunction = false;

            funcType = (LangtFunctionType)fn.TransformedType.PointeeType!;
            resultType = funcType!.ReturnType;

            var smatch = funcType.MatchSignature(state, Range, givenArgs);
            builder.AddData(smatch.OutResult);

            boundArgs = smatch.OutResult.WithDefault().ToArray();

            if(!builder)
            {
                return builder.WithDgnError(
                    $"Could not call a function pointer of type {funcType.Name} " +
                    $"with arguments of type {string.Join(", ", boundArgs.Select(a => (a?.TransformedType ?? LangtType.Error).GetFullName()))}",
                    Range
                ).Build<BoundASTNode>();
            }
        }
        else 
        {
            return builder.WithDgnError("Cannot call a non-functional expression", Range).Build<BoundASTNode>();
        }

        return builder.Build<BoundASTNode>
        (
            new BoundFunctionCall(this, fn, boundArgs!, hasDirectFunction, directFunction, funcType)
        );
    }
}