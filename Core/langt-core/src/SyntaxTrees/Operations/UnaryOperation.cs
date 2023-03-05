using Langt.Lexing;
using Langt.Message;
using Langt.Structure;
using Langt.Structure.Visitors;

using TT = Langt.Lexing.TokenType;

namespace Langt.AST;

public record UnaryOperation(ASTToken Operator, ASTNode Operand) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Operator, Operand};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        if(Operator.Type is TT.Ampersand or TT.Star) 
        {
            var vr = Operand.Bind(ctx, new TypeCheckOptions {AutoDeference = false});
            builder.AddData(vr);

            if(!builder) return builder.BuildError<BoundASTNode>();

            var v = vr.Value;

            if(Operator.Type is TT.Ampersand)
            {
                /*
                if(!v.Type.IsReference)
                {
                    return builder.WithDgnError($"Cannot get a pointer to a non-reference", Range).BuildError<BoundASTNode>();
                }
                */

                //TODO: THIS

                return builder.Build<BoundASTNode>
                (
                    new BoundPtrTo(this, v)
                );
            }
            else 
            {
                v = v.TryDeference();
                //TODO: THIS
                
                if(!v.Type.IsPointer)
                {
                    return builder.WithDgnError(Messages.Get("ptr-bad-deref"), Range).BuildError<BoundASTNode>();
                }

                return builder.Build<BoundASTNode>
                (
                    new BoundDereference(this, v)
                );
            }
        }

        var fn = ctx.GetOperator(new(Parsing.OperatorType.Unary, Operator.Type));
        var fr = fn.ResolveOverload(new[] {Operand}, Range, ctx);

        builder.AddData(fr);

        if(!fr) return builder.BuildError<BoundASTNode>();

        var fo = fr.Value;
        var fp = fo.OutputParameters.Value.ToArray();

        return builder.Build<BoundASTNode>
        (
            new BoundFunctionCall(this, fo.Function, fp)
        );
    }
}
