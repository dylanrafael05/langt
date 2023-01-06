public static class Readable
{
    public static string CommaListWithFinal(IEnumerable<string> values, string finalMarker)
    {
        var s = values.ToArray();

        if(s.Length == 0) return string.Empty;
        if(s.Length == 1) return s[0];
        if(s.Length == 2) return s[0] + " " + finalMarker + " " + s[1];
        
        return string.Join(", ", s[0..^2]) + ", " + finalMarker + s[^1];
    }

    public static string CommaListOr(IEnumerable<string> values)
        => CommaListWithFinal(values, "or");
    public static string CommaListAnd(IEnumerable<string> values)
        => CommaListWithFinal(values, "and");
}