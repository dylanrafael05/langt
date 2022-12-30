using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundGroup(ASTNode Source, IList<BoundASTNode> BoundNodes) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {BoundNodes};

    public override void LowerSelf(CodeGenerator generator)
    {
        foreach(var s in BoundNodes)
        {
            s.Lower(generator);
            generator.DiscardValues(DebugSourceName);
        }
    }

    public static Result<BoundASTNode> BindFromNodes(ASTNode source, IEnumerable<ASTNode> nodes, ASTPassState state)
    {
        var builder = ResultBuilder.Empty();
        var returns = false;

        var boundNotes = new List<BoundASTNode>();

        foreach(var n in nodes)
        {
            var res = n.Bind(state);
            builder.AddData(res);
            
            var bast = res.Or(new BoundASTWrapper(n) {IsError = true})!;

            if(returns)
            {
                bast.Unreachable = true;
            }

            returns |= bast.Returns;

            boundNotes.Add(bast);
        }

        if(!builder) return builder.Build<BoundASTNode>();
        
        return builder.Build<BoundASTNode>
        (
            new BoundGroup(source, boundNotes!)
            {
                RawExpressionType = LangtType.None
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
        => BoundGroup.BindFromNodes(this, Statements, state);
}
