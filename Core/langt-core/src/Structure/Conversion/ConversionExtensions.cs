namespace Langt.Structure;

public static class ConversionExtensions
{
    public static LangtType WinningType(this Context ctx, LangtType a, LangtType b, out LangtConversion? ab, out LangtConversion? ba)
    {
        ab = ba = null;
        if(a == b) return a;

        ab = ctx.ResolveConversion(a, b, SourceRange.Default).Map<LangtConversion?>(c => c).OrDefault();
        ba = ctx.ResolveConversion(b, a, SourceRange.Default).Map<LangtConversion?>(c => c).OrDefault();

        if(ab is null && ba is null) 
            throw new NotSupportedException($"Called .WinningType with two types that are not mutually convertible!");
        
        if(ab!.Value.IsImplicit == ba!.Value.IsImplicit) 
            throw new NotSupportedException($"Called .WinningType with two types that are mutually convertible by the same explicity!");

        var ret = ab is null || ab.Value.IsImplicit ? b : a;
        
        ab = ret == a ? null : ab;
        ba = ret == b ? null : ba;

        return ret;
    }
    public static LangtType WinningType(this Context ctx, LangtType a, LangtType b)
        => ctx.WinningType(a, b, out _, out _);
}