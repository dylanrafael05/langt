using Langt.Codegen;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

public record StatementGroup(IList<ASTNode> Statements) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Statements};
    public override bool BlockLike => true;

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Group");
        foreach(var s in Statements) 
        {
            visitor.Visit(s);
        }
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => BoundGroup.BindFromNodes(this, Statements, state);
}
