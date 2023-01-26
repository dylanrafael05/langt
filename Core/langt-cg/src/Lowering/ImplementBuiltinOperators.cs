using Langt.Structure;

using static LLVMSharp.Interop.LLVMIntPredicate;
using static LLVMSharp.Interop.LLVMRealPredicate;

namespace Langt.CG.Lowering;

public static class ImplementBuiltinOperators
{
    public delegate LLVMValueRef BinOp(LLVMValueRef a, LLVMValueRef b);
    public delegate LLVMValueRef UnOp(LLVMValueRef x);

    public static void Implement(CodeGenerator cg, LangtFunction fn, BinOp op) 
    {
        cg.Function(fn, () =>
        {
            cg.Builder.BuildRet(op(cg.CurrentFunction.GetParam(0), cg.CurrentFunction.GetParam(1)));
        });
    }
    public static void Implement(CodeGenerator cg, LangtFunction fn, UnOp op) 
    {
        cg.Function(fn, () =>
        {
            cg.Builder.BuildRet(op(cg.CurrentFunction.GetParam(0)));
        });
    }

    public static void Initialize(CodeGenerator cg)
    {
        IEnumerable<LangtFunction> Ops(params string[] ns) 
        {
            foreach(var n in ns)
                foreach(var o in cg
                    .Project
                    .GlobalScope
                    .ResolveFunctionGroup(n, SourceRange.Default)
                    .Expect()
                    .Overloads)
                    yield return o;
        }

        // Boolean 'not'
        foreach(var f in Ops(LangtWords.MagicNot))
        {
            if(f.Type.ParameterTypes is [{Name : LangtWords.Boolean}])
            {
                Implement(cg, f, x => cg.Builder.BuildNot(x));
            }
        }

        // Unary '-'
        foreach(var f in Ops(LangtWords.MagicNeg))
        {
            if(f.Type.ParameterTypes is [{IsInteger : true}])
            {
                Implement(cg, f, x => cg.Builder.BuildNeg(x));
            }

            if(f.Type.ParameterTypes is [{IsReal : true}])
            {
                Implement(cg, f, x => cg.Builder.BuildFNeg(x));
            }
        }

        // Binary '+', '-', '*', '/', '%', '==', '!=', '>', '>=', '<', '<='
        foreach(var f in Ops(LangtWords.MagicAdd, LangtWords.MagicSub, LangtWords.MagicMul, LangtWords.MagicDiv, LangtWords.MagicMod))
        {
            if(f.Type.ParameterTypes is not [var a, var b] || !a.IsInteger && !a.IsReal || !b.IsInteger && !b.IsReal)
            {
                continue;
            }

            var win = cg.Context.WinningType(a, b, out var convAB, out var convBA);
            var isI = win.IsInteger;
            
            Implement(cg, f, (x, y) =>
            {
                x = convAB.HasValue ? cg.Binder.Get(convAB!.Value)(x) : x;
                y = convBA.HasValue ? cg.Binder.Get(convBA!.Value)(y) : y;

                return f.Name switch 
                {
                    LangtWords.MagicAdd     => isI ? cg.Builder.BuildAdd(x, y)              : cg.Builder.BuildFAdd(x, y),
                    LangtWords.MagicSub     => isI ? cg.Builder.BuildSub(x, y)              : cg.Builder.BuildFSub(x, y),
                    LangtWords.MagicMul     => isI ? cg.Builder.BuildMul(x, y)              : cg.Builder.BuildFMul(x, y),

                    LangtWords.MagicDiv     => isI ? (win.Signedness is Signedness.Signed ? cg.Builder.BuildSDiv(x, y) : cg.Builder.BuildUDiv(x, y))
                                                   : cg.Builder.BuildFDiv(x, y),
                    LangtWords.MagicMod     => isI ? (win.Signedness is Signedness.Signed ? cg.Builder.BuildSRem(x, y) : cg.Builder.BuildURem(x, y))
                                                   : cg.Builder.BuildFRem(x, y),

                    LangtWords.MagicEq      => isI ? cg.Builder.BuildICmp(LLVMIntEQ, x, y)  : cg.Builder.BuildFCmp(LLVMRealOEQ, x, y),
                    LangtWords.MagicNotEq   => isI ? cg.Builder.BuildICmp(LLVMIntNE, x, y)  : cg.Builder.BuildFCmp(LLVMRealONE, x, y),

                    LangtWords.MagicLess    => isI ? (win.Signedness is Signedness.Signed ? cg.Builder.BuildICmp(LLVMIntSLT, x, y) : cg.Builder.BuildICmp(LLVMIntULT, x, y)) 
                                                   : cg.Builder.BuildFCmp(LLVMRealOLT, x, y),
                    LangtWords.MagicLessEq  => isI ? (win.Signedness is Signedness.Signed ? cg.Builder.BuildICmp(LLVMIntSLE, x, y) : cg.Builder.BuildICmp(LLVMIntULE, x, y)) 
                                                   : cg.Builder.BuildFCmp(LLVMRealOLE, x, y),
                    LangtWords.MagicGreat   => isI ? (win.Signedness is Signedness.Signed ? cg.Builder.BuildICmp(LLVMIntSGT, x, y) : cg.Builder.BuildICmp(LLVMIntUGT, x, y)) 
                                                   : cg.Builder.BuildFCmp(LLVMRealOGT, x, y),
                    LangtWords.MagicGreatEq => isI ? (win.Signedness is Signedness.Signed ? cg.Builder.BuildICmp(LLVMIntSGE, x, y) : cg.Builder.BuildICmp(LLVMIntUGE, x, y)) 
                                                   : cg.Builder.BuildFCmp(LLVMRealOGE, x, y),

                    _ => throw new NotSupportedException("Unreachable")
                };
            });
        }
    }
}