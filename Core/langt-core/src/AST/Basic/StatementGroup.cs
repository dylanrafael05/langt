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

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        foreach(var s in Statements)
        {
            s.TryTypeCheck(state);
        }

        RawExpressionType = LangtType.None;
    }

    public override void DefineFunctions(ASTPassState state)
    {
        foreach(var s in Statements)
        {
            TryPass(s.DefineFunctions, state);
        }
    }

    public override void ImplementTypes(ASTPassState state)
    {
        foreach(var s in Statements)
        {
            TryPass(s.ImplementTypes, state);
        }
    }
    
    public override void DefineTypes(ASTPassState state)
    {
        foreach(var s in Statements)
        {
            TryPass(s.DefineTypes, state);
        }
    }

    public override void Initialize(ASTPassState state)
    {
        foreach(var s in Statements)
        {
            TryPass(s.Initialize, state);
        }
    }
}
