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

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        LangtType exprType;
        LangtType? natType = null;
        bool isTTDep = false;

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

            if(options.TargetType is not null && options.TargetType.IsInteger)
            {
                // Get maximum size for target type
                var tt = options.TargetType.IntegerBitDepth;
                tt = tt is -1 ? 32 : tt;

                var maxVal = (tt, options.TargetType.Signedness) switch 
                {
                    (8,  Signedness.Signed  ) => (ulong) sbyte.MaxValue,
                    (8,  Signedness.Unsigned) => (ulong) byte.MaxValue,
                    (16, Signedness.Signed  ) => (ulong) short.MaxValue,
                    (16, Signedness.Unsigned) => (ulong) ushort.MaxValue,
                    (32, Signedness.Signed  ) => (ulong) int.MaxValue,
                    (32, Signedness.Unsigned) => (ulong) uint.MaxValue,
                    (64, Signedness.Signed  ) => (ulong) long.MaxValue,
                    (64, Signedness.Unsigned) => (ulong) ulong.MaxValue,

                    _  => throw new NotSupportedException("Unknown integer size")
                };

                // Throw an error if value does not fit within type
                if(intVal > maxVal) return Result.Error<BoundASTNode>
                (
                    Diagnostic.Error($"Integer {intVal} out of range for type {options.TargetType}", Range)
                );

                // Change expression and natural type to match target
                exprType = options.TargetType;
                natType  = null;

                isTTDep = true;
            }
        }
        else
        {
            dblVal = double.Parse(Tok.ContentStr);
            exprType = LangtType.Real32; //todo: better handling of floating point literals

            // Match target real type if possible
            if(options.TargetType is not null && options.TargetType.IsReal)
            {
                exprType = options.TargetType;
                isTTDep = true;
            }
        }

        // Create result
        var r = Result.Success<BoundASTNode>
        (
            new BoundNumericLiteral(this, intVal, dblVal)
            {
                Type = exprType,
                NaturalType = natType
            }
        );

        // And tag as target type dependent as necessary
        if(isTTDep) r = r.AsTargetTypeDependent();

        return r;
    }
}
