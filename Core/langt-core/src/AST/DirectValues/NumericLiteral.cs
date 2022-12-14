using Langt.Lexing;
using Langt.Codegen;
using Langt.Utility;
using Langt.Structure.Visitors;

namespace Langt.AST;


public record NumericLiteral(ASTToken Tok) : ASTNode, IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.PutString(Tok.ToString());

    public long? IntegerValue {get; set;}
    public double? DoubleValue {get; set;}

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        if(Tok.Type is TokenType.Integer)
        {
            IntegerValue = long.Parse(Tok.ContentStr);

            (RawExpressionType, NaturalType) = IntegerValue switch 
            {
                <= sbyte.MaxValue => (LangtType.Int8,  LangtType.Int32),
                <= short.MaxValue => (LangtType.Int16, LangtType.Int32),
                <= int  .MaxValue => (LangtType.Int32, LangtType.Int32),
                _                 => (LangtType.Int64, LangtType.Int64)
            };
        }
        else
        {
            DoubleValue = double.Parse(Tok.ContentStr);
            RawExpressionType = LangtType.Real32; //todo: better handling of floating point literals
        }
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(DoubleValue.HasValue)
        {
            lowerer.PushValue( 
                RawExpressionType,
                LLVMValueRef.CreateConstReal(lowerer.LowerType(RawExpressionType), DoubleValue!.Value),
                DebugSourceName
            );
        }
        else
        {
            lowerer.PushValue(
                RawExpressionType,
                LLVMValueRef.CreateConstInt(lowerer.LowerType(RawExpressionType), ULongFormat.I64(IntegerValue!.Value)),
                DebugSourceName
            );
        }
    }
}
