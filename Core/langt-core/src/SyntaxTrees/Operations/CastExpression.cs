using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record CastExpression(ASTNode Value, ASTToken As, ASTType Type) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Value, As, Type};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Cast");
        visitor.Visit(Value);
        visitor.Visit(Type);
    }

    public ITransformer? Transformer {get; private set;}

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        Value.TypeCheck(state);

        var type = Type.Resolve(state);
        if(type is null) return;

        var conversion = state.CG.ResolveConversion(type, Value.TransformedType);

        if(conversion is null)
        {
            state.Error($"Could not find a conversion from {Value.TransformedType.Name} to {type.Name}", Range);
            return;
        }

        Transformer = conversion.TransformProvider.TransformerFor(Value.TransformedType, type);
        RawExpressionType = type;
    }

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