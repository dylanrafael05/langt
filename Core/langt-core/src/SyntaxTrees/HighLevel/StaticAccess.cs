using Langt.Message;
using Langt.Structure;


namespace Langt.AST;

public record StaticAccess(ASTNode Left, ASTToken Dot, ASTToken Right) : ASTNode 
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Dot, Right};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        // Get all input results
        var iptResult = Left.Bind(ctx, new TypeCheckOptions {AutoDeference = false});
        if(!iptResult) return iptResult;

        // Create output result builder from input
        var builder = ResultBuilder.Empty().WithData(iptResult);

        // Deconstruct and get values
        var left = iptResult.Value;
        var hasResolution = left.HasResolution;

        // Fail if attempting to perform a static access on something which is not a scope
        if(!hasResolution || left.Resolution is not IScope ns)
        {
            return builder.WithDgnError(Messages.Get("cc-bad-lhs"), Range)
                .BuildError<BoundASTNode>();
        }

        // Attempt to grab result
        var resolutionResult = ns.ResolveDirect(Right.ContentStr, Range, ctx);
        builder.AddData(resolutionResult);

        if(!builder) return builder.BuildError<BoundASTNode>();

        // Return a BoundEmpty wrapping this and a static reference for what has been gotten
        return builder.Build<BoundASTNode>
        (
            new BoundEmpty(this)
            {
                Resolution = resolutionResult.Value,
                HasResolution = true
            }
        )
        .AddStaticReference(Right.Range, resolutionResult.Value);
    }
}
