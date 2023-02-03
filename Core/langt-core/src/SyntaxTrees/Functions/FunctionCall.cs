using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundFunctionPointerCall(ASTNode Source, BoundASTNode Function, BoundASTNode[] Arguments, LangtFunctionType FunctionType) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Function, Arguments};
}

public record BoundFunctionCall(ASTNode Source, LangtFunction Function, BoundASTNode[] Arguments) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Arguments};
    public override LangtType Type => Function.Type.ReturnType;
}

public record FunctionCall(ASTNode FunctionAST, ASTToken Open, SeparatedCollection<ASTNode> Arguments, ASTToken End) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {FunctionAST, Open, Arguments, End};

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

            if(!resolveResult) return builder.BuildError<BoundASTNode>();

            var resolution = resolveResult.Value;
            builder.AddData(resolution.OutputParameters);

            boundArgs = resolution.OutputParameters.Value.ToArray();

            builder.ModifyBindingOptions(b => 
            {
                var r = iptResult.GetBindingOptions().References;

                foreach(var k in r.OfType<StaticReference>().Where(k => k.Item is LangtFunctionGroup).ToArray())
                {
                    b.References.Remove(k);
                    b.References.Add(k with {Item = resolution.Function});
                }

                return b;
            });

            return builder.Build<BoundASTNode>
            (
                new BoundFunctionCall(this, resolution.Function, boundArgs)
                {
                    Type = resolution.Function.Type.ReturnType
                }
            );
        }
        else if(fn.Type.IsFunctionPtr)
        {
            var funcType = (LangtFunctionType)fn.Type.ElementType!;

            var smatch = funcType.MatchSignature(state, Range, givenArgs);
            builder.AddData(smatch.OutResult);

            boundArgs = smatch.OutResult.Value;

            if(!builder)
            {
                return builder.WithDgnError(
                    $"Could not call a function pointer of type {funcType.Name} " +
                    $"with arguments of type {string.Join(", ", boundArgs.Select(a => (a?.Type ?? LangtType.Error).FullName))}",
                    Range
                ).BuildError<BoundASTNode>();
            }

            return builder.Build<BoundASTNode>
            (
                new BoundFunctionPointerCall(this, fn, boundArgs, funcType)
                {
                    Type = funcType.ReturnType
                }
            );
        }
        else 
        {
            return builder.WithDgnError("Cannot call a non-functional expression", Range).BuildError<BoundASTNode>();
        }
    }
}