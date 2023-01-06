using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundCastExpression(CastExpression Source, BoundASTNode Value, BoundASTNode Type, ITransformer Transformer) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value, Type};
    public override void LowerSelf(CodeGenerator lowerer)
    {
        Value.Lower(lowerer);

        var v = lowerer.PopValue(DebugSourceName);

        lowerer.PushValue( 
            Transformer!.Output,
            Transformer.Perform(lowerer, v.LLVM),
            DebugSourceName
        );
    }
}

public record CastExpression(ASTNode Value, ASTToken As, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Value, As, Type};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Cast");
        visitor.Visit(Value);
        visitor.Visit(Type);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var results = Result.All
        (
            Value.Bind(state),
            Type.Bind(state) 
        );
        var builder = ResultBuilder.From(results);

        if(!results) return builder.Build<BoundASTNode>();

        var (val, boundType) = results.Value;

        var type = (LangtType)boundType.Resolution!;

        var cv = state.CG.ResolveConversion(type, val.TransformedType, Range);
        builder.AddData(cv);

        if(!cv) return builder.Build<BoundASTNode>();
        var conversion = cv.Value;

        // TODO: make RawExpressionType required or a parameter
        return builder.Build<BoundASTNode>
        (
            new BoundCastExpression(this, val, boundType, conversion.TransformProvider.TransformerFor(val.TransformedType, type))
            {
                RawExpressionType = type
            }
        );
    }
}