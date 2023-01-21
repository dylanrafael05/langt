using Langt.Structure;

namespace Langt.AST;

public record CompilationUnit(StatementGroup Group) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Group};

    public override bool BlockLike => true;

    
    public override Result HandleDefinitions(ASTPassState state)
        => Group.HandleDefinitions(state);
    public override Result RefineDefinitions(ASTPassState state)
        => Group.RefineDefinitions(state);
    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Group.Bind(state);
}
