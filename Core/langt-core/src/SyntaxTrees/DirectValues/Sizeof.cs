namespace Langt.AST;

public record Sizeof(ASTToken SizeofTok, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {SizeofTok, Type};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
        => Type.GetSymbol(ctx).Unravel(ctx).Map<BoundASTNode>(t => new BoundSizeof(this, t));
}
