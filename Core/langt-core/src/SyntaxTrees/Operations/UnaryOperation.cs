using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record UnaryOperation(ASTToken Operator, ASTNode Operand) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Operator, Operand};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString($"Unary {Operator.ContentStr}");
        visitor.Visit(Operand);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var fn = state.CG.GetOperator(new(Parsing.OperatorType.Unary, Operator.Type));
        var fr = fn.ResolveOverload(new[] {Operand}, Range, state);

        var builder = ResultBuilder.From(fr);

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
