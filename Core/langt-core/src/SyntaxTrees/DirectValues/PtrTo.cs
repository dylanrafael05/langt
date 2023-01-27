using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundPtrTo(UnaryOperation Source, BoundASTNode Value) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
    public override LangtType Type => LangtPointerType.Create(Value.Type.ElementType!).Expect("Invalid pointer type");
}
public record BoundDereference(UnaryOperation Source, BoundASTNode Value) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
    public override LangtType Type => Value.Type.ElementType!;
}


[Obsolete("Use UnaryOperator with '&' instead", true)]
public record PtrTo(ASTToken Ptr, ASTToken Var) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Ptr, Var};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Pointer to " + Var.ContentStr);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var vResult = state.CTX.ResolutionScope.ResolveVariable
        (
            Var.ContentStr, 
            Range
        );

        if(!vResult) return vResult.ErrorCast<BoundASTNode>();

        return Result.Success<BoundASTNode>(new BoundPtrTo(null!, null!)
        {
            Type = LangtPointerType.Create(vResult.Value.Type.ElementType!)
                .Expect("Errors should not occur while getting a pointer to a variable")
        });
    }
}