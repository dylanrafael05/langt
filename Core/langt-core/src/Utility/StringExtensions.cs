using System.Numerics;

namespace Langt.Utility;

public static class StringExtensions
{
    public static string Stringify<T>(this IEnumerable<T> ipt, Func<T, object?> stringifier, string sep = ", ")
        => string.Join(sep, ipt.Select(stringifier).Select(s => s?.ToString()));
    public static string Stringify(this IEnumerable<object?> ipt, string sep = ", ")
        => string.Join(sep, ipt.Select(k => k?.ToString() ?? "<null>"));
    public static string Stringify(this IEnumerable<string?> ipt, string sep = ", ")
        => string.Join(sep, ipt);

    public static string PaddedSubstring(this string s, int start, int end) 
        => " ".Repeat(-start) + s[Math.Max(0, start)..Math.Min(end, s.Length)] + " ".Repeat(end - s.Length);

    public static string ReString(this IEnumerable<char> ipt) 
        => new(ipt.ToArray());

    public static string Pluralize<T>(this string str, string pluralForm, T num)
        where T: INumber<T>
    {
        if(num == T.One || num == -T.One)
        {
            return str;
        }

        return pluralForm;
    }

    public static string Pluralize<T>(this string str, T num)
        where T: INumber<T>
        => str.Pluralize(str + "s", num);
    
    public static string Repeat(this string str, int count) 
        => string.Concat(Enumerable.Repeat(str, Math.Max(count, 0)));
}