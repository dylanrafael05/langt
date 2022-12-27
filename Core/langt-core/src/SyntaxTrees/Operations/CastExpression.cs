using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundCastExpression(CastExpression Source, BoundASTNode Value, ITransformer Transformer) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value};
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
            Type.Resolve(state) 
        );
        var builder = ResultBuilder.From(results);

        if(!results) return builder.Build<BoundASTNode>();

        var (val, type) = results.Value;

        var cv = state.CG.ResolveConversion(type, val.TransformedType, Range);
        builder.AddData(cv);

        if(!cv) return builder.Build<BoundASTNode>();
        var conversion = cv.Value;

        return builder.Build<BoundASTNode>
        (
            new BoundCastExpression(this, val, conversion.TransformProvider.TransformerFor(val.TransformedType, type))
        );
    }
}