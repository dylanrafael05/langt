using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record Block(ASTToken Open, IList<ASTNode> Statements, ASTToken Close) : ASTNode
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Open, Statements, Close};
    public override bool BlockLike => true;

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Block {");
        foreach(var s in Statements) 
        {
            visitor.Visit(s);
        }
        visitor.PutString("}");
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        foreach(var s in Statements)
        {
            if(!Returns) 
            {
                s.TryTypeCheck(state);
            }
            else
            {
                s.Unreachable = true;
            }

            Returns |= s.Returns;
        }
        
        RawExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        foreach(var s in Statements)
        {
            s.Lower(lowerer);
            lowerer.DiscardValues(DebugSourceName);
        }
    }
    
    public override void DefineTypes(ASTPassState state)
    {
        foreach(var s in Statements)
        {
            TryPass(s.DefineTypes, state);
        }
    }

    public override void ImplementTypes(ASTPassState state)
    {
        foreach(var s in Statements)
        {
            TryPass(s.ImplementTypes, state);
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
