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

            (ExpressionType, InferrableType) = IntegerValue switch 
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
            ExpressionType = LangtType.Real32; //todo: better handling of floating point literals
        }
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(DoubleValue.HasValue)
        {
            lowerer.PushValue(
                ExpressionType,
                LLVMValueRef.CreateConstReal(lowerer.LowerType(ExpressionType), DoubleValue!.Value)
            );
        }
        else
        {
            lowerer.PushValue(
                ExpressionType,
                LLVMValueRef.CreateConstInt(lowerer.LowerType(ExpressionType), ULongFormat.I64(IntegerValue!.Value))
            );
        }
    }
}
