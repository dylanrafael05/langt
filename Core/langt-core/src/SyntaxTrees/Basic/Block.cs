using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Resolutions;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundGroup(ASTNode Source, IList<BoundASTNode> BoundNodes, IScope? Scope) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {BoundNodes};
    
    
    [MemberNotNullWhen(true, nameof(Scope))] 
    public bool HasScope => Scope is not null;

    public static Result<BoundASTNode> BindFromNodes(ASTNode source, IEnumerable<ASTNode> nodes, ASTPassState state, IScope? scope = null)
    {
        var builder = ResultBuilder.Empty();
        var returns = false;
        var anyUnreachableLog = false;

        var boundNotes = new List<BoundASTNode>();

        foreach(var n in nodes)
        {
            var res = n.Bind(state);
            builder.AddData(res);
            
            var bast = res.Or(new BoundEmpty(n) {IsError = true})!;

            if(returns)
            {
                if(!anyUnreachableLog)
                {
                    builder.AddWarning("Unreachable code", bast.Range);
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

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Block {");
        foreach(var s in Statements) 
        {
            visitor.Visit(s);
        }
        visitor.PutString("}");
    }

    public override Result HandleDefinitions(ASTPassState state)
        => ResultGroup.GreedyForeach(Statements, n => n.HandleDefinitions(state)).Combine();
    public override Result RefineDefinitions(ASTPassState state)
        => ResultGroup.GreedyForeach(Statements, n => n.RefineDefinitions(state)).Combine();

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var scope = options.PredefinedBlockScope ?? state.CTX.OpenScope();

        var r = BoundGroup.BindFromNodes(this, Statements, state, scope);

        if(!options.HasPredefinedBlockScope) state.CTX.CloseScope();

        return r;
    }
}
