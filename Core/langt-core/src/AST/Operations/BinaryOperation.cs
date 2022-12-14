using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

using TT = Langt.Lexing.TokenType;

namespace Langt.AST;

public record BinaryOperation(ASTNode Left, ASTToken Operator, ASTNode Right) : ASTNode(), IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {Left, Operator, Right};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString($"Binary {Operator.Range.Content}");
        visitor.Visit(Left);
        visitor.Visit(Right);
    }

    public static LangtType? Dominate(CodeGenerator generator, ASTNode l, ASTNode r, SourceRange range)
    {
        if(!generator.MakeMatch(l.TransformedType, r))
        {
            if(!generator.MakeMatch(r.TransformedType, l))
            {
                generator.Diagnostics.Error($"Cannot perform any binary operations on values of types {l.TransformedType.Name} and {r.TransformedType.Name}", range);
                return null;
            }
            else
            {
                return l.TransformedType;
            }
        }
        else
        {
            return r.TransformedType;
        }
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Left.TypeCheck(generator);
        Right.TypeCheck(generator);

        DominantType = Dominate(generator, Left, Right, Range);

        if(DominantType is null) return;

        if(!DominantType.IsInteger && !DominantType.IsReal && DominantType != LangtType.Bool)
        {
            generator.Diagnostics.Error($"Operation {Operator.ContentStr} is unsupported for values of type {DominantType.Name}", Range);
        }

        static LangtType Dummy(Action action)
        {
            action();
            return LangtType.None;
        }

        RawExpressionType = Operator.Type switch
        {
            TT.Plus or TT.Minus or TT.Star or TT.Slash or TT.Percent 
                => DominantType,
            TT.DoubleEquals or TT.NotEquals or TT.GreaterThan or TT.LessThan or TT.GreaterEqual or TT.LessEqual or TT.And or TT.Or 
                => LangtType.Bool,
            _ => Dummy(() => generator.Diagnostics.Error($"Unknown operation {Operator.ContentStr}", Range))
        };
    }

    public LangtType? DominantType {get; private set;}

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Right.Lower(lowerer);
        Left.Lower(lowerer);

        var (l, r) = (lowerer.PopValue(DebugSourceName), lowerer.PopValue(DebugSourceName));

        LLVMValueRef Dummy(Action action)
        {
            action();
            return default;
        }

        LLVMValueRef v;

        if(l.Type.IsReal)
        {
            // TODO: MOVE CHECKING TO TYPE CHECKER
            v = Operator.Type switch 
            {
                TT.Plus    => lowerer.Builder.BuildFAdd(l.LLVM, r.LLVM, "radd"),
                TT.Minus   => lowerer.Builder.BuildFSub(l.LLVM, r.LLVM, "rsub"),
                TT.Star    => lowerer.Builder.BuildFMul(l.LLVM, r.LLVM, "rmul"),
                TT.Slash   => lowerer.Builder.BuildFDiv(l.LLVM, r.LLVM, "rdiv"),
                TT.Percent => lowerer.Builder.BuildFRem(l.LLVM, r.LLVM, "rmod"),

                TT.DoubleEquals => lowerer.Builder.BuildFCmp(LLVMRealPredicate.LLVMRealUEQ, l.LLVM, r.LLVM, "rcmp"),
                TT.NotEquals    => lowerer.Builder.BuildFCmp(LLVMRealPredicate.LLVMRealUNE, l.LLVM, r.LLVM, "rcmp"),
                TT.GreaterThan  => lowerer.Builder.BuildFCmp(LLVMRealPredicate.LLVMRealUGT, l.LLVM, r.LLVM, "rcmp"),
                TT.GreaterEqual => lowerer.Builder.BuildFCmp(LLVMRealPredicate.LLVMRealUGE, l.LLVM, r.LLVM, "rcmp"),
                TT.LessThan     => lowerer.Builder.BuildFCmp(LLVMRealPredicate.LLVMRealULT, l.LLVM, r.LLVM, "rcmp"),
                TT.LessEqual    => lowerer.Builder.BuildFCmp(LLVMRealPredicate.LLVMRealULE, l.LLVM, r.LLVM, "rcmp"),

                _ => Dummy(() => lowerer.Diagnostics.Error($"Cannot perform operation {Operator.ContentStr} on integers", Range))
            };
        }
        else if(l.Type.IsInteger)
        {
            v = Operator.Type switch 
            {
                TT.Plus    => lowerer.Builder.BuildAdd (l.LLVM, r.LLVM, "siadd"),
                TT.Minus   => lowerer.Builder.BuildSub (l.LLVM, r.LLVM, "sisub"),
                TT.Star    => lowerer.Builder.BuildMul (l.LLVM, r.LLVM, "simul"),
                TT.Slash   => lowerer.Builder.BuildSDiv(l.LLVM, r.LLVM, "sidiv"),
                TT.Percent => lowerer.Builder.BuildSRem(l.LLVM, r.LLVM, "simod"),

                TT.DoubleEquals => lowerer.Builder.BuildICmp(LLVMIntPredicate.LLVMIntEQ, l.LLVM, r.LLVM, "sicmp"),
                TT.NotEquals    => lowerer.Builder.BuildICmp(LLVMIntPredicate.LLVMIntNE, l.LLVM, r.LLVM, "sicmp"),
                TT.GreaterThan  => lowerer.Builder.BuildICmp(LLVMIntPredicate.LLVMIntSGT, l.LLVM, r.LLVM, "sicmp"),
                TT.GreaterEqual => lowerer.Builder.BuildICmp(LLVMIntPredicate.LLVMIntSGE, l.LLVM, r.LLVM, "sicmp"),
                TT.LessThan     => lowerer.Builder.BuildICmp(LLVMIntPredicate.LLVMIntSLT, l.LLVM, r.LLVM, "sicmp"),
                TT.LessEqual    => lowerer.Builder.BuildICmp(LLVMIntPredicate.LLVMIntSLE, l.LLVM, r.LLVM, "sicmp"),

                _ => Dummy(() => lowerer.Diagnostics.Error($"Cannot perform operation {Operator.ContentStr} on reals", Range))
            };
        }
        else if(l.Type == LangtType.Bool)
        {
            v = Operator.Type switch 
            {
                TT.And => lowerer.Builder.BuildAnd(l.LLVM, r.LLVM, "booland"),
                TT.Or  => lowerer.Builder.BuildOr (l.LLVM, r.LLVM, "boolor"),

                _ => Dummy(() => lowerer.Diagnostics.Error($"Cannot perform operation {Operator.ContentStr} on booleans", Range))
            };
        }
        else
        {
            throw new Exception("this should theoretically be unreachable!");
        }

        lowerer.PushValue(RawExpressionType, v, DebugSourceName);
    }
}
