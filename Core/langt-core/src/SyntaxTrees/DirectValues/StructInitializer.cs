using Langt.Lexing;
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

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var tn = Type.Resolve(state);
        var builder = ResultBuilder.From(tn);
        if(!tn) return tn.ErrorCast<BoundASTNode>();

        var type = tn.Value;

        if(!type.IsStructure)
        {
            return builder.WithDgnError($"Unknown structure type {type.Name}", Range).BuildError<BoundASTNode>();
        }

        var args = Args.Values.ToList();

        if(type.Structure!.Fields.Count != args.Count)
        {
            return builder.WithDgnError($"Incorrect number of fields for structure initializer of type {type.Name}", Range)
                .BuildError<BoundASTNode>();
        }

        var results = ResultGroup.GreedyForeach
        (
            args.Indexed(),
            a => 
            {
                var f = type.Structure!.Fields[a.Index];
                
                var ftype = f.Type;
                var fname = f.Name;

                var r = a.Value.BindMatchingExprType(state, ftype);

                if(!r) return Result.Error<BoundASTNode>
                (
                    Diagnostic.Error($"Incorrect type for field '{fname}' in struct initializer for struct {type.Name}", Range)
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