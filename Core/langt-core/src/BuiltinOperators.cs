namespace Langt.Structure;

using Langt.AST;
using TT = Langt.Lexing.TokenType;

public static class BuiltinOperators
{
    public static void Initialize(Context ctx)
    {
        // NOT //
        ctx.DefineUnaryOperator(TT.Not, LangtType.Bool, LangtType.Bool);

        // NEGATION //
        foreach(var t in LangtType.SignedIntegerTypes.Concat(LangtType.RealTypes))
        {
            ctx.DefineUnaryOperator(TT.Minus, t, t);
        }
        
        ctx.DefineUnaryOperator(TT.Minus, LangtType.UInt8,  LangtType.Int16);
        ctx.DefineUnaryOperator(TT.Minus, LangtType.UInt16, LangtType.Int32);
        ctx.DefineUnaryOperator(TT.Minus, LangtType.UInt32, LangtType.Int64);

        // BINARY OPS //
        void Create(LangtType a, LangtType b, LangtType r, TT op)
        {
            ctx.DefineBinaryOperator(op, a, b, r);
            if(a != b) ctx.DefineBinaryOperator(op, b, a, r);
        }

        // COMPARISONS //
        foreach(var (a, b) in 
                    LangtType.AllIntegerTypes.Choose(LangtType.RealTypes)
            .Concat(LangtType.AllIntegerTypes.ChooseSelfUnique())
            .Concat(LangtType.RealTypes.ChooseSelfUnique()))
        {
            Create(a, b, LangtType.Bool, TT.DoubleEquals);
            Create(a, b, LangtType.Bool, TT.NotEquals);
            Create(a, b, LangtType.Bool, TT.LessThan);
            Create(a, b, LangtType.Bool, TT.LessEqual);
            Create(a, b, LangtType.Bool, TT.GreaterThan);
            Create(a, b, LangtType.Bool, TT.GreaterEqual);
        }

        // ARITHMETIC //
        foreach(var (a, b) in 
                    LangtType.AllIntegerTypes.Choose(LangtType.RealTypes)
            .Concat(LangtType.SignedIntegerTypes.ChooseSelfUnique())
            .Concat(LangtType.UnsignedIntegerTypes.ChooseSelfUnique())
            .Concat(LangtType.RealTypes.ChooseSelfUnique()))
        {
            LangtType win;

            if(a.IsNativeInteger || b.IsNativeInteger)
            {
                if(a.IsNativeInteger) win = a;
                else                  win = b;
            }
            else win = ctx.WinningType(a, b);

            Create(a, b, win, TT.Plus);
            Create(a, b, win, TT.Minus);
            Create(a, b, win, TT.Star);
            Create(a, b, win, TT.Slash);
            Create(a, b, win, TT.Percent);
        }
    }
}