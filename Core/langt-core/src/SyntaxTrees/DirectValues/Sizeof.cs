namespace Langt.AST;

public record Sizeof(ASTToken SizeofTok, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {SizeofTok, Type};

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Type.Resolve(state).Map<BoundASTNode>(t => new BoundSizeof(this, t));
}
