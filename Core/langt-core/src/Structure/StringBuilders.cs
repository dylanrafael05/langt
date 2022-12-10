namespace Langt;

public static class StringBuilders
{
    public static StringBuilder AppendLayered(this StringBuilder b, int depth, string txt)
        => b.Append(string.Join("", Enumerable.Repeat(".   ", depth))).Append(txt).AppendLine();
    public static StringBuilder AppendLayered(this StringBuilder b, int depth, ReadOnlySpan<char> txt)
        => b.Append(string.Join("", Enumerable.Repeat(".   ", depth))).Append(txt).AppendLine();
}