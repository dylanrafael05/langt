using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record StatementGroup(IList<ASTNode> Statements) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Statements};
    public override bool BlockLike => true;

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Group");
        foreach(var s in Statements) 
        {
            visitor.Visit(s);
        }
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        foreach(var s in Statements)
        {
            s.Lower(lowerer);
            lowerer.DiscardValues(DebugSourceName);
        }
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        foreach(var s in Statements)
        {
            s.TypeCheck(generator);
        }

        RawExpressionType = LangtType.None;
    }

    public override void DefineFunctions(CodeGenerator generator)
    {
        foreach(var s in Statements)
        {
            s.DefineFunctions(generator);
        }
    }

    public override void ImplementTypes(CodeGenerator generator)
    {
        foreach(var s in Statements)
        {
            s.ImplementTypes(generator);
        }
    }
    
    public override void DefineTypes(CodeGenerator generator)
    {
        foreach(var s in Statements)
        {
            s.DefineTypes(generator);
        }
    }

    public override void Initialize(CodeGenerator generator)
    {
        foreach(var s in Statements)
        {
            s.Initialize(generator);
        }
    }
}
