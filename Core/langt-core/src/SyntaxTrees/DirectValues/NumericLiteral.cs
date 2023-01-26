using Langt.Lexing;
using Langt.Structure;
using Langt.Utility;
using Langt.Structure.Visitors;
using System.Numerics;

namespace Langt.AST;

public record BoundNumericLiteral(NumericLiteral Source, ulong? IntegerValue, double? DoubleValue) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
}

public record NumericLiteral(ASTToken Tok) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.PutString(Tok.ToString());

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        LangtType exprType;
        LangtType? natType = null;

        // TODO: reimplement using BigInteger / BigFloat
        ulong? intVal = null;
        double? dblVal = null;

        if(Tok.Type is TokenType.Integer)
        {
            intVal = ulong.Parse(Tok.ContentStr);

            (exprType, natType) = intVal switch 
            {
                <= byte  .MaxValue => (LangtType.Int8,  LangtType.Int32),
                <= ushort.MaxValue => (LangtType.Int16, LangtType.Int32),
                <= uint  .MaxValue => (LangtType.Int32, LangtType.Int32),
                _                  => (LangtType.Int64, LangtType.Int64)
            };
        }
        else
        {
            dblVal = double.Parse(Tok.ContentStr);
            exprType = LangtType.Real32; //todo: better handling of floating point literals
        }

        return Result.Success<BoundASTNode>
        (
            new BoundNumericLiteral(this, intVal, dblVal)
            {
                Type = exprType,
                NaturalType = natType
            }
        );
    }
}
