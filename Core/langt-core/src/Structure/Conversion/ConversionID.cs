namespace Langt.Structure;

public record struct ConversionID(string Name, ConversionType Type)
{
    public bool IsImplicit => Type is ConversionType.Implicit or ConversionType.Internal;
    public bool IsCallable => Type is ConversionType.Explicit or ConversionType.Implicit;
    
    public static ConversionID Identity    => new("a->a",   ConversionType.Implicit);
    public static ConversionID PointerCast => new("*a->*b", ConversionType.Explicit);
    public static ConversionID Deference   => new("&a->a",  ConversionType.Internal);
    public static ConversionID AliasConst  => new("a->'a",  ConversionType.Explicit);
    public static ConversionID AliasDest   => new("'a->a",  ConversionType.Explicit);
    public static ConversionID OptionConst => new("a->a|.", ConversionType.Implicit);
}
