using Langt.AST;

namespace Langt.Structure;

public class FunctionalConversionProvider : IConversionProvider
{
    private readonly Func<LangtType, LangtType, bool> pred;
    private readonly ConversionID id;

    private FunctionalConversionProvider(Func<LangtType, LangtType, bool> pred, ConversionID id) 
    {
        this.pred = pred;
        this.id   = id;
    }

    public LangtConversion? GetConversionFor(LangtType input, LangtType output) 
        => pred(input, output) ? (new(input, output, id)) : null;

    public static IConversionProvider Predicate(Func<LangtType, LangtType, bool> pred, ConversionID id)
        => new FunctionalConversionProvider(pred, id);
    public static IConversionProvider Direct(LangtType input, LangtType output, ConversionType id) 
        => Predicate((i, o) => i == input && o == output, new(input.FullName+"->"+output.FullName, id));
}