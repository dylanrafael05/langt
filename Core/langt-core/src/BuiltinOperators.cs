namespace Langt.Structure;

using Langt.AST;
using TT = Langt.Lexing.TokenType;

public static class BuiltinOperators
{
    public static void Initialize(Context cg)
    {
        // NOT //
        cg.DefineUnaryOperator(TT.Not, LangtType.Bool, LangtType.Bool);

        // NEGATION //
        foreach(var t in LangtType.IntegerTypes.Concat(LangtType.RealTypes))
        {
            cg.DefineUnaryOperator(TT.Minus, t, t);
        }

        // BINARY-OPS //
        foreach(var (a, b) in 
                    LangtType.IntegerTypes.Choose(LangtType.RealTypes)
            .Concat(LangtType.IntegerTypes.ChooseSelfUnique())
            .Concat(LangtType.RealTypes   .ChooseSelfUnique()))
        {
            var win = cg.WinningType(a, b);

            void Create(TT op)
            {
                cg.DefineBinaryOperator(op, a, b, win);
                if(a != b) cg.DefineBinaryOperator(op, b, a, win);
            }

            Create(TT.Plus);
            Create(TT.Minus);
            Create(TT.Star);
            Create(TT.Slash);
            Create(TT.Percent);
            Create(TT.DoubleEquals);
            Create(TT.NotEquals);
            Create(TT.LessThan);
            Create(TT.LessEqual);
            Create(TT.GreaterThan);
            Create(TT.GreaterEqual);
        }

        // TODO: implement non-arithmetic equality operators
    }
}