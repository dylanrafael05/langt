using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record Block(ASTToken Open, IList<ASTNode> Statements, ASTToken Close) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Open, Statements, Close};
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

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        foreach(var s in Statements)
        {
            if(!Returns) 
            {
                s.TypeCheck(generator);
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
    
    public override void DefineTypes(CodeGenerator generator)
    {
        foreach(var s in Statements)
        {
            s.DefineTypes(generator);
        }
    }

    public override void ImplementTypes(CodeGenerator generator)
    {
        foreach(var s in Statements)
        {
            s.ImplementTypes(generator);
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
