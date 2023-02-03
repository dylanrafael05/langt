using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ElseStatement(ASTToken Else, ASTNode End) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Else, End};

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => End.Bind(state, options);
}
