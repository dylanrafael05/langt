namespace Langt.Structure;

public interface IConversionProvider
{
    LangtConversion? GetConversionFor(LangtType input, LangtType output);
}
