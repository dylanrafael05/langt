namespace Langt.Codegen;

using Langt.AST;
using TT = Langt.Lexing.TokenType;

public static class BuiltinOperators
{
    public static void Initialize(CodeGenerator generator)
    {
        // NEGATION //
        foreach(var r in LangtType.IntegerTypes)
        {
            generator.DefineUnaryOperator(TT.Minus, r, r, (b, x) => b.BuildNeg(x));
        }

        foreach(var r in LangtType.RealTypes)
        {
            generator.DefineUnaryOperator(TT.Minus, r, r, (b, x) => b.BuildFNeg(x));
        }

        // int+real //
        foreach(var (i, r) in LangtType.IntegerTypes.Choose(LangtType.RealTypes))
        {
            var convIR = generator.ResolveConversion(r, i, SourceRange.Default).Expect().TransformProvider.TransformerFor(i, r);

            void Create(TT op, CodeGenerator.BinaryOpDefiner definer)
            {
                generator.DefineBinaryOperator(op, i, r, r, (b, x, y) => definer(b, convIR.Perform(generator, x), y));
                generator.DefineBinaryOperator(op, r, i, r, (b, x, y) => definer(b, x, convIR.Perform(generator, y)));
            }

            void CreateComp(TT op, LLVMRealPredicate pred)
            {
                generator.DefineBinaryOperator(op, i, r, LangtType.Bool, (b, x, y) => b.BuildFCmp(pred, convIR.Perform(generator, x), y));
                generator.DefineBinaryOperator(op, r, i, LangtType.Bool, (b, x, y) => b.BuildFCmp(pred, x, convIR.Perform(generator, y)));
            }

            Create(TT.Plus,    (build, a, b) => build.BuildFAdd(a, b));
            Create(TT.Minus,   (build, a, b) => build.BuildFSub(a, b));
            Create(TT.Star,    (build, a, b) => build.BuildFMul(a, b));
            Create(TT.Slash,   (build, a, b) => build.BuildFDiv(a, b));
            Create(TT.Percent, (build, a, b) => build.BuildFRem(a, b));

            CreateComp(TT.DoubleEquals, LLVMRealPredicate.LLVMRealOEQ);
            CreateComp(TT.NotEquals,    LLVMRealPredicate.LLVMRealONE);
            CreateComp(TT.LessThan,     LLVMRealPredicate.LLVMRealOLT);
            CreateComp(TT.LessEqual,    LLVMRealPredicate.LLVMRealOLE);
            CreateComp(TT.GreaterThan,  LLVMRealPredicate.LLVMRealOGT);
            CreateComp(TT.GreaterEqual, LLVMRealPredicate.LLVMRealOGE);
        }
        
        // int+int //
        foreach(var (i1, i2) in LangtType.IntegerTypes.ChooseSelf())
        {
            ITransformer? convmM = null;
            if(i1 != i2)
            {
                var conv21 = generator.ResolveConversion(i1, i2, SourceRange.Default).Expect()!;
                var conv12 = generator.ResolveConversion(i2, i1, SourceRange.Default).Expect()!;

                convmM = conv12.IsImplicit 
                    ? conv12.TransformProvider.TransformerFor(i1, i2) 
                    : conv21.TransformProvider.TransformerFor(i2, i1)
                    ;
            }

            var im = convmM?.Input == i1 ? i1 : i2;
            var iM = convmM?.Input == i1 ? i2 : i1;

            void Create(TT op, CodeGenerator.BinaryOpDefiner definer)
            {
                if(convmM is not null)
                {
                    generator.DefineBinaryOperator(op, i1, i2, iM, (b, x, y) => definer
                    (
                        b, 
                        convmM.Input == i1 ? convmM.Perform(generator, x) : x, 
                        convmM.Input == i2 ? convmM.Perform(generator, y) : y
                    ));
                }
                else
                {
                    generator.DefineBinaryOperator(op, i1, i1, i1, (b, x, y) => definer(b, x, y));
                }
            }

            void CreateComp(TT op, LLVMIntPredicate pred)
            {
                if(convmM is not null)
                {
                    generator.DefineBinaryOperator(op, i1, i2, LangtType.Bool, (b, x, y) => b.BuildICmp
                    (
                        pred, 
                        convmM.Input == i1 ? convmM.Perform(generator, x) : x, 
                        convmM.Input == i2 ? convmM.Perform(generator, y) : y
                    ));
                }
                else 
                {
                    generator.DefineBinaryOperator(op, i1, i1, LangtType.Bool, (b, x, y) => b.BuildICmp(pred, x, y));
                }
            }

            Create(TT.Plus,    (build, a, b) => build.BuildAdd(a, b));
            Create(TT.Minus,   (build, a, b) => build.BuildSub(a, b));
            Create(TT.Star,    (build, a, b) => build.BuildMul(a, b));
            Create(TT.Slash,   (build, a, b) => build.BuildSDiv(a, b));
            Create(TT.Percent, (build, a, b) => build.BuildSRem(a, b));

            CreateComp(TT.DoubleEquals, LLVMIntPredicate.LLVMIntEQ);
            CreateComp(TT.NotEquals,    LLVMIntPredicate.LLVMIntNE);
            CreateComp(TT.LessThan,     LLVMIntPredicate.LLVMIntSLT);
            CreateComp(TT.LessEqual,    LLVMIntPredicate.LLVMIntSLE);
            CreateComp(TT.GreaterThan,  LLVMIntPredicate.LLVMIntSGT);
            CreateComp(TT.GreaterEqual, LLVMIntPredicate.LLVMIntSGE);
        }

        // real+real //
        foreach(var (i1, i2) in LangtType.RealTypes.ChooseSelf())
        {
            ITransformer? convmM = null;
            if(i1 != i2)
            {
                var conv21 = generator.ResolveConversion(i1, i2, SourceRange.Default).Expect();
                var conv12 = generator.ResolveConversion(i2, i1, SourceRange.Default).Expect();

                convmM = conv12.IsImplicit 
                    ? conv12.TransformProvider.TransformerFor(i1, i2) 
                    : conv21.TransformProvider.TransformerFor(i2, i1)
                    ;
            }

            var im = convmM?.Input == i1 ? i1 : i2;
            var iM = convmM?.Input == i1 ? i2 : i1;

            void Create(TT op, CodeGenerator.BinaryOpDefiner definer)
            {
                if(convmM is not null)
                {
                    generator.DefineBinaryOperator(op, i1, i2, iM, (b, x, y) => definer
                    (
                        b, 
                        convmM.Input == i1 ? convmM.Perform(generator, x) : x, 
                        convmM.Input == i2 ? convmM.Perform(generator, y) : y
                    ));
                }
                else
                {
                    generator.DefineBinaryOperator(op, i1, i1, i1, (b, x, y) => definer(b, x, y));
                }
            }

            void CreateComp(TT op, LLVMRealPredicate pred)
            {
                if(convmM is not null)
                {
                    generator.DefineBinaryOperator(op, i1, i2, LangtType.Bool, (b, x, y) => b.BuildFCmp
                    (
                        pred, 
                        convmM.Input == i1 ? convmM.Perform(generator, x) : x, 
                        convmM.Input == i2 ? convmM.Perform(generator, y) : y
                    ));
                }
                else 
                {
                    generator.DefineBinaryOperator(op, i1, i1, LangtType.Bool, (b, x, y) => b.BuildFCmp(pred, x, y));
                }
            }

            Create(TT.Plus,    (build, a, b) => build.BuildFAdd(a, b));
            Create(TT.Minus,   (build, a, b) => build.BuildFSub(a, b));
            Create(TT.Star,    (build, a, b) => build.BuildFMul(a, b));
            Create(TT.Slash,   (build, a, b) => build.BuildFDiv(a, b));
            Create(TT.Percent, (build, a, b) => build.BuildFRem(a, b));

            CreateComp(TT.DoubleEquals, LLVMRealPredicate.LLVMRealOEQ);
            CreateComp(TT.NotEquals,    LLVMRealPredicate.LLVMRealONE);
            CreateComp(TT.LessThan,     LLVMRealPredicate.LLVMRealOLT);
            CreateComp(TT.LessEqual,    LLVMRealPredicate.LLVMRealOLE);
            CreateComp(TT.GreaterThan,  LLVMRealPredicate.LLVMRealOGT);
            CreateComp(TT.GreaterEqual, LLVMRealPredicate.LLVMRealOGE);
        }

        // not X //
        generator.DefineUnaryOperator(TT.Not, LangtType.Bool, LangtType.Bool, (b, a) => b.BuildNot(a));
    }
}