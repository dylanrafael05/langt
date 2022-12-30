using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundFunctionPointerCall(ASTNode Source, BoundASTNode Function, BoundASTNode[] Arguments, LangtFunctionType FunctionType) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Function, Arguments};

    public override void LowerSelf(CodeGenerator generator)
    {
        Function.Lower(generator);

        generator.BuildFunctionCall
        (
            generator.PopValue(DebugSourceName).LLVM,
            Arguments,
            FunctionType,
            DebugSourceName
        );
    }
}

public record BoundFunctionCall(ASTNode Source, LangtFunction Function, BoundASTNode[] Arguments) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Arguments};

    public override void LowerSelf(CodeGenerator lowerer)
    {
        lowerer.BuildFunctionCall
        (
            Function.LLVMFunction,
            Arguments,
            Function.Type,
            DebugSourceName
        );
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
        BoundASTNode[] boundArgs;

        if(fn.HasResolution && fn.Resolution is LangtFunctionGroup functionGroup)
        {
            var resolveResult = functionGroup.ResolveOverload(givenArgs, Range, state);
            builder.AddData(resolveResult);

            if(!resolveResult) return builder.Build<BoundASTNode>();

            var resolution = resolveResult.Value;

            boundArgs = resolution.OutputParameters.Value.ToArray();

            return builder.Build<BoundASTNode>
            (
                new BoundFunctionCall(this, resolution.Function, boundArgs)
                {
                    RawExpressionType = resolution.Function.Type.ReturnType
                }
            );
        }
        else if(fn.TransformedType.IsFunctionPtr)
        {
            var funcType = (LangtFunctionType)fn.TransformedType.PointeeType!;

            var smatch = funcType.MatchSignature(state, Range, givenArgs);
            builder.AddData(smatch.OutResult);

            boundArgs = smatch.OutResult.Value;

            if(!builder)
            {
                return builder.WithDgnError(
                    $"Could not call a function pointer of type {funcType.Name} " +
                    $"with arguments of type {string.Join(", ", boundArgs.Select(a => (a?.TransformedType ?? LangtType.Error).GetFullName()))}",
                    Range
                ).Build<BoundASTNode>();
            }

            return builder.Build<BoundASTNode>
            (
                new BoundFunctionPointerCall(this, fn, boundArgs, funcType)
                {
                    RawExpressionType = funcType.ReturnType
                }
            );
        }
        else 
        {
            return builder.WithDgnError("Cannot call a non-functional expression", Range).Build<BoundASTNode>();
        }
    }
}