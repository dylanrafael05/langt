using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BooleanLiteral(ASTToken Tok) : ASTNode, IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.PutString(Tok.ToString());

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        RawExpressionType = LangtType.Bool;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        lowerer.PushValue( 
            LangtType.Bool,
            LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, Tok.Type is TokenType.True ? 1ul : 0ul),
            DebugSourceName
        );
    }
}
