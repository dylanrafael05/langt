using Langt.Codegen;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

public record BoundGroup(ASTNode Source, IList<BoundASTNode> Items) : BoundASTNode(Source)
{
    public override void LowerSelf(CodeGenerator generator)
    {
        foreach(var i in Items) 
        {
            i.Lower(generator);
        }
    }
}

public record StatementGroup(IList<ASTNode> Statements) : ASTNode
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Statements};
    public override bool BlockLike => true;

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Group");
        foreach(var s in Statements) 
        {
            visitor.Visit(s);
        }
    }
}
