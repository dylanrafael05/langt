namespace Langt.Structure;

public record struct LangtConversion(LangtType Input, LangtType Output, ConversionID ID) : IConversionProvider
{
    public override string ToString() => ID.Name;

    public bool IsImplicit => ID.IsImplicit;
    public bool IsCallable => ID.IsCallable;

    LangtConversion? IConversionProvider.GetConversionFor(LangtType input, LangtType output) 
        => input == Input && output == Output ? this : null;

    public static readonly IReadOnlyList<IConversionProvider> Builtins;
    public static IConversionProvider Dereference {get;}

    public static LangtConversion DereferenceFor(LangtType ty) 
        => Dereference.GetConversionFor(ty, ty.ElementType!) ?? throw new Exception($"Expected valid dereference canditate, got {ty}");

    static LangtConversion()
    {
        Dereference = FunctionalConversionProvider.Predicate((i, o) => i.IsReference && o == i.ElementType, ConversionID.Deference);

        var n = new List<IConversionProvider>
        {
            Dereference,

            FunctionalConversionProvider.Predicate((i, o) => i.IsPointer && o.IsPointer, ConversionID.PointerCast),
            FunctionalConversionProvider.Predicate((i, o) => o.IsAlias && i == o.AliasBaseType, ConversionID.AliasConst),
            FunctionalConversionProvider.Predicate((i, o) => i.IsAlias && o == i.AliasBaseType, ConversionID.AliasDest),
            FunctionalConversionProvider.Predicate((i, o) => i.IsOption && i.OptionTypes.Contains(o), ConversionID.OptionConst),
        };

        void GeneratePromotions(IEnumerable<LangtType> source, Func<LangtType, int> bitDepth)
        {
            foreach(var (input, output) in source.ChooseSelf())
            {
                if(bitDepth(input) == bitDepth(output))
                {
                    continue;
                }

                var convTy = bitDepth(input) > bitDepth(output) ? ConversionType.Explicit : ConversionType.Implicit;

                if(input.IsNativeInteger)       convTy = ConversionType.Explicit;
                else if(output.IsNativeInteger) convTy = bitDepth(input) < 32 ? ConversionType.Implicit : ConversionType.Explicit;

                n!.Add
                (
                    FunctionalConversionProvider.Direct(input, output, convTy)
                );
            }
        }

        GeneratePromotions(LangtType.UnsignedIntegerTypes, t => t.IntegerBitDepth!.Value);
        GeneratePromotions(LangtType.SignedIntegerTypes,   t => t.IntegerBitDepth!.Value);
        GeneratePromotions(LangtType.RealTypes,            t => t.RealBitDepth!.Value);

        foreach(var (sTy, uTy) in LangtType.SignedIntegerTypes.Choose(LangtType.UnsignedIntegerTypes))
        {
            n.Add(FunctionalConversionProvider.Direct(sTy, uTy, ConversionType.Explicit));
            n.Add(FunctionalConversionProvider.Direct(uTy, sTy, ConversionType.Explicit));
        }

        foreach(var (intTy, realTy) in LangtType.AllIntegerTypes.Choose(LangtType.RealTypes))
        {
            n.Add(FunctionalConversionProvider.Direct(intTy, realTy, ConversionType.Implicit));
            n.Add(FunctionalConversionProvider.Direct(realTy, intTy, ConversionType.Explicit));
        }

        Builtins = n;
    }
}
