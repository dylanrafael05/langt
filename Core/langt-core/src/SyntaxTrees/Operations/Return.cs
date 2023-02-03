using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundReturn(Return Source, BoundASTNode? Value) : BoundASTNode(Source) 
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value};
}

public record Return(ASTToken ReturnTok, ASTNode? Value = null) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {ReturnTok, Value};

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        if(Value is null) return Result.Success<BoundASTNode>(new BoundReturn(this, null) {Returns = true});

        var rtype = state.CTX.CurrentFunction!.Type.ReturnType;

        var vr = Value.BindMatchingExprType(state, rtype);
        if(!vr) return vr;

        var builder = ResultBuilder.From(vr);

        return builder.Build<BoundASTNode>
        (
            new BoundReturn(this, vr.Value)
            {
                Returns = true
            }
        );
    }
}