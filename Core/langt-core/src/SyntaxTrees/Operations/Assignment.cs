using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundStructFieldAssignment(Assignment Source, BoundStructFieldAccess Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public BoundASTNode LeftValue => Left.Left;
    public int FieldIndex => Left.FieldIndex;
    public LangtStructureField Field => Left.Field;
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};
    public override LangtType Type => LangtType.None;
}

public record BoundVariableAssignment(Assignment Source, BoundVariableReference Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public LangtVariable Variable => Left.Variable;
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};
    public override LangtType Type => LangtType.None;
}

public record BoundDereferenceAssignment(Assignment Source, BoundDereference Left, BoundASTNode Right) : BoundASTNode(Source)
{
    public BoundASTNode LeftValue => Left.Value;
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left, Right};
    public override LangtType Type => LangtType.None;
}

/************************************************

    Assignment Rules:

    Values must be 'assignable' in order to be
    the left-hand operand of '='

    All 'assignable' values fall into four categories:

    x = 0     # variable assign
    *x = 0    # dereference assign
    x[y] = 0  # index assign
    x.y = 0   # dot access assign, when x is an l-value

    It is assumed that all members of these categories
    produce true for '.IsAssignable'

    Any assignable value must also be represented
    using a pointer in output LLVM IR, since assignment
    is represented using a 'load' instruction

*************************************************/

public record Assignment(ASTNode Left, ASTToken Assign, ASTNode Right) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Assign, Right};

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        if(Left is IndexExpression idx)
        {
            return BindIndexAssign(idx, state);
        }
        
        var builder = ResultBuilder.Empty();

        var leftResult = Left.Bind(state, new TypeCheckOptions {AutoDeference = false});
        builder.AddData(leftResult);
        if(!leftResult) return builder.BuildError<BoundASTNode>();

        var left = leftResult.Value;
        
        if(!left.IsAssignable)
        {
            return builder.WithDgnError($"Cannot assign to a non-assignable value", Range).BuildError<BoundASTNode>();
        }

        var rightResult = Right.BindMatchingExprType(state, left.Type.ElementType!);
        builder.AddData(rightResult);
        if(!rightResult) return builder.BuildError<BoundASTNode>();

        var right = rightResult.Value;

        return builder.Build<BoundASTNode>
        (
            left switch
            {
                BoundVariableReference bvr  => new BoundVariableAssignment(this, bvr, right),
                BoundDereference bd         => new BoundDereferenceAssignment(this, bd, right),
                BoundStructFieldAccess bsfa => new BoundStructFieldAssignment(this, bsfa, right),

                _ => throw new NotSupportedException($"Unknown assignable bound node type {left.GetType()}")
            }
        );
    }

    private Result<BoundASTNode> BindIndexAssign(IndexExpression idxExpr, ASTPassState state)
    {
        // TODO! create and use Context.BindGlobalFunctionCall(ASTNode[] args, string name)

        var builder = ResultBuilder.Empty();

        var op = state.CTX.GetGlobalFunction(LangtWords.MagicSetIndex);

        var args = idxExpr.Arguments.Values
            .Prepend(idxExpr.Value)
            .Append(Right)
            .ToArray();

        var fr = op.ResolveOverload(args, Range, state);
        builder.AddData(fr);

        if(!builder) return builder.BuildError<BoundASTNode>();

        var fn = fr.Value;
        var outputParams = fn.OutputParameters.Value;

        if(!outputParams[0].IsAssignable)
        {
            return builder.WithDgnError($"Cannot index-assign to a non-assignable variable", Range)
                .BuildError<BoundASTNode>();
        }

        return builder.Build<BoundASTNode>
        (
            new BoundFunctionCall(this, fn.Function, outputParams)
        );
    }
}
