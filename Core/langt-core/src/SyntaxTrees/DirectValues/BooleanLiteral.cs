using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundBooleanLiteral(BooleanLiteral Source) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
}

public record BooleanLiteral(ASTToken Tok) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Tok};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
        => Result.Success<BoundASTNode>(new BoundBooleanLiteral(this) {Type = LangtType.Bool});
}
