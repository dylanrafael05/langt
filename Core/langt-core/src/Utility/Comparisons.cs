namespace Langt.Utility;

public static class Comparisons
{
    public static bool BetweenInclusive<T>(T low, T value, T high) where T : IComparable<T>
        => low.CompareTo(value) <= 0 && value.CompareTo(high) <= 0;

    public static bool BetweenExclusive<T>(T low, T value, T high) where T : IComparable<T>
        => low.CompareTo(value) < 0 && value.CompareTo(high) < 0;
}