using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ParentheticExpression(ASTToken Open, ASTNode Value, ASTToken End) : ASTNode(), IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Open, Value, End};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
        => Value.Bind(ctx, options);
}
