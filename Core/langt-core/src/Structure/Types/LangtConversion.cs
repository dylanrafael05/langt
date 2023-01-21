using Langt.Utility;
using Langt.AST;
using Langt.Structure;

namespace Langt.Structure;

public record LangtConversion(TransformProvider TransformProvider)
{
    public bool IsImplicit {get; init;}

    public static readonly List<LangtConversion> Builtin = new();

    static LangtConversion()
    {
        void Add(TransformProvider provider, bool isImplicit = false)
        {
            Builtin.Add(new(provider) {IsImplicit = isImplicit});
        }

        var intTypes = LangtType.IntegerTypes;
        var realTypes = LangtType.RealTypes;

        void BuildConverters(
            IEnumerable<LangtType> source, 
            Func<LangtType, int?> bitDepth, 
            Func<LLVMBuilderRef, LLVMValueRef, LLVMTypeRef, string, LLVMValueRef> ext, 
            Func<LLVMBuilderRef, LLVMValueRef, LLVMTypeRef, string, LLVMValueRef> trunc
        )
        {
            foreach(var (to, from) in source.ChooseSelf())
            {
                if(bitDepth(to) == bitDepth(from))
                {
                    continue;
                }

                Add(new DirectTransformProvider(to, from,
                    bitDepth(from) < bitDepth(to)
                        ? (cg, v) => ext  (cg.Builder, v, cg.LowerType(to), from.Name + ".to." + to.Name)
                        : (cg, v) => trunc(cg.Builder, v, cg.LowerType(to), from.Name + ".to." + to.Name)
                ), isImplicit: bitDepth(from) < bitDepth(to));
            }
        }

        BuildConverters(
            intTypes, 
            t => t.IntegerBitDepth, 
            (b, v, t, n) => b.BuildSExt(v, t, n), 
            (b, v, t, n) => b.BuildTrunc(v, t, n)
        );
        BuildConverters(
            realTypes, 
            t => t.RealBitDepth, 
            (b, v, t, n) => b.BuildFPExt(v, t, n),
            (b, v, t, n) => b.BuildFPTrunc(v, t, n)
        );

        foreach(var (to, from) in realTypes.Choose(intTypes))
        {
            Add(new DirectTransformProvider(to, from, 
                (cg, v) => cg.Builder.BuildSIToFP(v, cg.LowerType(to), from.Name + ".to." + to.Name)
            ), isImplicit: true);
        }

        foreach(var (to, from) in intTypes.Choose(realTypes))
        {
            string fromName = "f" + from.RealBitDepth!.Value;
            string toName   = "i" + to.IntegerBitDepth!.Value;

            string lroundName = "llvm.nearbyint." + fromName;

            var lFuncType = LangtFunctionType.Create(new[] {from}, from).Expect();

            // TODO: make accessing intrinsics easier
            Add(new DirectTransformProvider(to, from,
                (cg, v) => cg.Builder.BuildFPToSI(
                    cg.Builder.BuildCall2(
                        cg.LowerType(lFuncType), 
                        cg.GetIntrinsic(lroundName, lFuncType), 
                        new[] {v}, 
                        from.Name + ".round"
                    ),
                    cg.LowerType(to),
                    from.Name + ".to." + to.Name
                )
            ));
        }

        // Pointer conversions //
        Add(new FunctionalTransformProvider(
            (t1, t2) => t1.IsPointer && t2.IsPointer && t1.ElementType != t2.ElementType,
            (_, _, _, v) => v,
            "*a->*b"
        ));

        Add(new FunctionalTransformProvider(
            (t1, t2) => t1.IsAlias && t1.AliasBaseType == t2,
            (_, _, _, v) => v,
            "(alias a)->a"
        ));

        Add(new FunctionalTransformProvider(
            (t1, t2) => t2.IsAlias && t2.AliasBaseType == t1,
            (_, _, _, v) => v,
            "a->(alias a)"
        ));

        Add(new FunctionalTransformProvider(
            (t1, t2) => t2.IsOption && t2.OptionTypes.Contains(t1),
            (i, o, cg, v) =>
            {
                var s = cg.Builder.BuildAlloca(cg.LowerType(o));
                cg.Builder.BuildStore(v, s);
                
                var t = cg.Builder.BuildStructGEP2(cg.LowerType(o), s, LangtOptionType.TagLocation);
                cg.Builder.BuildStore(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, (ulong)o.OptionTypeMap![i]), t);

                return cg.Builder.BuildLoad2(cg.LowerType(o), s);
            },
            "a->a|..."
        ), true);

        // foreach(var b in Builtin)
        // {
        //     Messages.Info((b.IsImplicit ? "Implicit" 
        //                                 : "Explicit") + " conversion " + b.TransformProvider.Name + " exists!");
        // }
    }
}