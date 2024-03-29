using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ParentheticExpression(ASTToken Open, ASTNode Value, ASTToken End) : ASTNode(), IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Open, Value, End};

    public override void Dump(VisitDumper visitor)
        => visitor.VisitNoDepth(Value);

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Value.Bind(state, options);
}
