namespace Langt;

public static class StringExtensions
{
    public static string Stringify<T>(this IEnumerable<T> ipt, Func<T, object?> stringifier, string sep = ", ")
        => string.Join(sep, ipt.Select(stringifier).Select(s => s?.ToString()));
    public static string Stringify(this IEnumerable<string?> ipt, string sep = ", ")
        => string.Join(sep, ipt);
    
    public static string Repeat(this string str, int count) 
        => string.Concat(Enumerable.Repeat(str, count));

    public static StringBuilder AppendLayered(this StringBuilder b, int depth, string txt)
        => b.Append(".   ".Repeat(depth)).Append(txt).AppendLine();
    public static StringBuilder AppendLayered(this StringBuilder b, int depth, ReadOnlySpan<char> txt)
        => b.Append(".   ".Repeat(depth)).Append(txt).AppendLine();
}