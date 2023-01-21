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

    public override void Dump(VisitDumper visitor)
        => visitor.PutString(Tok.ToString());

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Result.Success<BoundASTNode>(new BoundBooleanLiteral(this) {Type = LangtType.Bool});
}
