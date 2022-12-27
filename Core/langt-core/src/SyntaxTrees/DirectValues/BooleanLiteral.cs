using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundBooleanLiteral(BooleanLiteral Source) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};

    public override void LowerSelf(CodeGenerator generator)
    {
        generator.PushValue( 
            LangtType.Bool,
            LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, Source.Tok.Type is TokenType.True ? 1ul : 0ul),
            DebugSourceName
        );
    }
}

public record BooleanLiteral(ASTToken Tok) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.PutString(Tok.ToString());

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
        => Result.Success<BoundASTNode>(new BoundBooleanLiteral(this) {RawExpressionType = LangtType.Bool});
}
