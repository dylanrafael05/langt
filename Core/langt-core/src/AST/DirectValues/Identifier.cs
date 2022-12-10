using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record Identifier(ASTToken Tok) : ASTNode, IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.VisitNoDepth(Tok);

    public override bool Readable => true;

    public bool IsVariable => Resolution is (not null) and LangtVariable;
    public LangtVariable? Variable => Resolution as LangtVariable;

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        HasResolution = true;
        Resolution = generator.ResolutionScope.Resolve(Tok.ContentStr, Range, generator.Diagnostics);
        
        if(!IsVariable) return;

        ExpressionType = LangtType.PointerTo(Variable!.Type);

        Variable.UseCount++;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        lowerer.PushValue(
            ExpressionType, Variable!.UnderlyingValue!.LLVM
        );
    }
}
