using Langt.Lexing;
using Langt.Message;
using Langt.Structure;

using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundGroup(ASTNode Source, IList<BoundASTNode> BoundNodes, IScope? Scope) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {BoundNodes};
    
    
    [MemberNotNullWhen(true, nameof(Scope))] 
    public bool HasScope => Scope is not null;

    public static Result<BoundASTNode> BindFromNodes(ASTNode source, IEnumerable<ASTNode> nodes, Context ctx, IScope? scope = null)
    {
        var builder = ResultBuilder.Empty();
        var returns = false;
        var anyUnreachableLog = false;

        var boundNotes = new List<BoundASTNode>();

        foreach(var n in nodes)
        {
            var res = n.Bind(ctx);
            builder.AddData(res);
            
            var bast = res.Or(new BoundEmpty(n) {IsError = true})!;

            if(returns)
            {
                if(!anyUnreachableLog)
                {
                    builder.AddWarning(Messages.Get("unreachable"), bast.Range);
                }

                bast.Unreachable = true;
                anyUnreachableLog = true;
            }

            returns |= bast.Returns;

            boundNotes.Add(bast);
        }

        if(!builder) return builder.BuildError<BoundASTNode>();
        
        return builder.Build<BoundASTNode>
        (
            new BoundGroup(source, boundNotes!, scope)
            {
                Type = LangtType.None,
                Returns = returns
            }
        );
    }
}

public record Block(ASTToken Open, IList<ASTNode> Statements, ASTToken Close) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Open, Statements, Close};
    public override bool BlockLike => true;

    public override Result HandleDefinitions(Context ctx)
        => ResultGroup.GreedyForeach(Statements, n => n.HandleDefinitions(ctx)).Combine();
    public override Result RefineDefinitions(Context ctx)
        => ResultGroup.GreedyForeach(Statements, n => n.RefineDefinitions(ctx)).Combine();

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        var scope = options.PredefinedBlockScope ?? ctx.OpenScope();

        var r = BoundGroup.BindFromNodes(this, Statements, ctx, scope);

        if(!options.HasPredefinedBlockScope) ctx.CloseScope();

        return r;
    }
}
