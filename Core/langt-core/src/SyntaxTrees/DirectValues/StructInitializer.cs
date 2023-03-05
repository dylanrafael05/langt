using Langt.Lexing;
using Langt.Message;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundStructInitializer(StructInitializer Source, BoundASTNode[] BoundArgs) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {BoundArgs};
}

public record StructInitializer(ASTType Type, ASTToken Open, SeparatedCollection<ASTNode> Args, ASTToken Close) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Type, Open, Args, Close};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        var tn = Type.UnravelSymbol(ctx);
        var builder = ResultBuilder.From(tn);
        if(!tn) return tn.ErrorCast<BoundASTNode>();

        var type = tn.Value;

        if(!type.IsStructure)
        {
            return builder.WithDgnError(Messages.Get("struct-init-not-struct", type), Range).BuildError<BoundASTNode>();
        }

        var args = Args.Values.ToList();
        var fieldCount = type.Structure!.Fields().Count();

        if(fieldCount != args.Count)
        {
            return builder.WithDgnError(Messages.Get("struct-init-bad-arg-count", this, fieldCount, args.Count), Range)
                .BuildError<BoundASTNode>();
        }

        var results = ResultGroup.GreedyForeach
        (
            args.Indexed(),
            a => 
            {
                // TODO: improve performance here!
                var f = type.Structure!.Fields().First(f => f.Index == a.Index);
                
                var ftype = f.Type;
                var fname = f.Name;

                var r = a.Value.BindMatchingExprType(ctx, ftype);

                if(!r) return Result.Error<BoundASTNode>
                (
                    Diagnostic.Error(Messages.Get("struct-init-bad-arg-type", this, fname, ftype), Range)
                );

                return r;
            }
        );
        builder.AddData(results);

        if(!results) return builder.BuildError<BoundASTNode>();

        return builder.Build<BoundASTNode>
        (
            new BoundStructInitializer(this, results.Value.ToArray())
            {
                Type = type
            }
        );

        // TODO: add another syntax tree? resolved syntax tree?
    }
}