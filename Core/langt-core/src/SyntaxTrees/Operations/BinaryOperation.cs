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

public record BoundPointerArith(BinaryOperation Source, bool IsAdd, BoundASTNode Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};
}
public record BoundPointerDiff(BinaryOperation Source, BoundASTNode Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};
}


public record BinaryOperation(ASTNode Left, ASTToken Operator, ASTNode Right) : ASTNode(), IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Operator, Right};

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();
        var invertResult = false;

        // Boolean 'and'/'or' //
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

        // Get function overload //
        var fn = state.CTX.GetOperator(new(Parsing.OperatorType.Binary, Operator.Type));
        var fr = fn.ResolveOverload(new[] {Left, Right}, Range, state);

        // Pointer arithmetic //
        if(Operator.Type is TT.Plus or TT.Minus && !fr) 
        {
            // Get left and right (usize)
            var (lr, rr) = (Left.Bind(state), Right.Bind(state));

            // Pointer diff
            if(Operator.Type is TT.Plus && !!lr && !!rr && lr.Value.Type == rr.Value.Type && lr.Value.Type.IsPointer)
            {
                return builder.Build<BoundASTNode>
                (
                    new BoundPointerDiff(this, lr.Value, rr.Value)
                    {
                        Type = LangtType.IntSZ
                    }
                );
            }

            // Pointer arith
            if(rr) rr = rr.Map(t => t.MatchExprType(state, LangtType.UIntSZ));
            if(rr) rr = rr.Map(t => t.MatchExprType(state, LangtType.IntSZ));

            builder.AddData(lr);
            builder.AddData(rr);

            if(builder) 
            {
                var (l, r) = (lr.Value, rr.Value);

                if(l.Type.IsPointer && r.Type.IsInteger)
                {
                    return builder.Build<BoundASTNode>
                    (
                        new BoundPointerArith(this, Operator.Type is TT.Plus, lr.Value, rr.Value) 
                        {
                            Type = lr.Value.Type
                        }
                    );
                }
            }
        }

        // Not-equals generation //
        if(Operator.Type is TT.NotEquals && !fr)
        {
            fr = state.CTX.GetOperator(new(Parsing.OperatorType.Binary, TT.DoubleEquals)).ResolveOverload(new[] {Left, Right}, Range, state);
            invertResult = true;
        }

        // Finalize //
        builder.AddData(fr);

        if(!fr) return builder.BuildError<BoundASTNode>();

        var fo = fr.Value;
        var fp = fo.OutputParameters.Value.ToArray();

        var res = new BoundFunctionCall(this, fo.Function, fp);

        if(invertResult) 
        {
            var not = state.CTX.GetOperator(new(Parsing.OperatorType.Unary, TT.Not))
                .ResolveExactOverload(new[] {LangtType.Bool}, false, Range)
                .Expect()
                .Function;
            
            res = new BoundFunctionCall(this, not, new[] {res});          
        }

        return builder.Build<BoundASTNode>(res);
    }
}
