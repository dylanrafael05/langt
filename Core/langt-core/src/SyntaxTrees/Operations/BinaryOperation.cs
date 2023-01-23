using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

using TT = Langt.Lexing.TokenType;

namespace Langt.AST;

public record BoundAndExpression(BinaryOperation Source, BoundASTNode Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};
}
public record BoundOrExpression(BinaryOperation Source, BoundASTNode Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};
}

public record BinaryOperation(ASTNode Left, ASTToken Operator, ASTNode Right) : ASTNode(), IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Operator, Right};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString($"Binary {Operator.Range.Content}");
        visitor.Visit(Left);
        visitor.Visit(Right);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        if(Operator.Type is TT.And or TT.Or)
        {
            var results = Result.GreedyAll
            (
                Left.BindMatchingExprType(state, LangtType.Bool),
                Right.BindMatchingExprType(state, LangtType.Bool)
            );
            builder.AddData(results);

            if(!results) return builder.BuildError<BoundASTNode>();

            var (l, r) = results.Value;

            return builder.Build<BoundASTNode>
            (
                Operator.Type is TT.And 
                    ? new BoundAndExpression(this, l, r)
                    : new BoundOrExpression (this, l, r)
            );
        }

        var fn = state.CTX.GetOperator(new(Parsing.OperatorType.Binary, Operator.Type));
        var fr = fn.ResolveOverload(new[] {Left, Right}, Range, state);

        builder.AddData(fr);

        if(!fr) return builder.BuildError<BoundASTNode>();

        var fo = fr.Value;
        var fp = fo.OutputParameters.Value.ToArray();

        return builder.Build<BoundASTNode>
        (
            new BoundFunctionCall(this, fo.Function, fp)
            {
                Type = fo.Function.Type.ReturnType
            }
        );
    }
}
