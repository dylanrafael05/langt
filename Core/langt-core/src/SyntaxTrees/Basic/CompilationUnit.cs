using Langt.Codegen;

namespace Langt.AST;

public record CompilationUnit(StatementGroup Group) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Group};

    public override bool BlockLike => true;

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Group.Bind(state);
}
