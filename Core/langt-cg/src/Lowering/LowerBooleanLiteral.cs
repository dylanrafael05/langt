using Langt.AST;
using Langt.Lexing;
using Langt.Structure;

namespace Langt.CG.Lowering;

public struct LowerBooleanLiteral : ILowerImplementation<BoundBooleanLiteral>
{
    public void LowerImpl(CodeGenerator cg, BoundBooleanLiteral node)
    {
        cg.PushValue( 
            LangtType.Bool,
            LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, node.Source.Tok.Type is TokenType.True ? 1ul : 0ul),
            node.DebugSourceName
        );
    }
}