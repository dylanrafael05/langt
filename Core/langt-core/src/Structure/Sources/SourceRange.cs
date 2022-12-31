namespace Langt;

/// <summary>
/// Represents a contiguous segment of a source object by line, character, and column.
/// May be Default, indicating no source is available.
/// </summary>
public readonly record struct SourceRange(int CharStart, int LineStart, int ColumnStart, int CharEnd, int LineEnd, int ColumnEnd, Source Source)
{
    public SourcePosition Start => new(CharStart, LineStart, ColumnStart, Source);
    public SourcePosition End => new(CharEnd, LineEnd, ColumnEnd, Source);

    public bool IsDefault => CharStart == -1;
    public static SourceRange Default => new(-1, -1, -1, -1, -1, -1, new("", ""));

    public int Length => CharEnd - CharStart;

    public override string ToString()
    {
        if(IsDefault) return "unknown";

        if(LineStart == LineEnd) 
        {
            return $"{Source.Name}:line {LineStart}";
        }
        else 
        {
            return $"{Source.Name}:line {LineStart} to line {LineEnd}";
        }
    }

    public ReadOnlySpan<char> Content => !IsDefault && CharStart >= 0 && CharEnd <= Source.Content?.Length 
        ? Source.Content[CharStart..Math.Min(CharEnd, Source.Content.Length)] 
        : (IsDefault ? "unknown" : "EOF");

    public static SourceRange From(SourcePosition start, SourcePosition end) 
    {
        if(start.Char > end.Char) throw new InvalidOperationException("Cannot create a range where the start is after the end!");
        if(start.Source != end.Source) throw new InvalidOperationException("Cannot create a range which spans multiple sources!");

        return new(start.Char, start.Line, start.Column, end.Char, end.Line, end.Column, start.Source);
    }

    public bool Contains(SourcePosition position) 
        => CharStart >= position.Char && position.Char >= CharEnd;
    public bool Contains(SourceRange range) 
        => Contains(range.Start) && Contains(range.End);

    public static SourceRange Combine(IEnumerable<SourceRange?> ranges) 
    {
        var vRanges = ranges.Where(r => r.HasValue && !r.Value.IsDefault);

        if(!vRanges.Any()) 
            return Default;

        var minStart = vRanges.Select(r => r!.Value.Start).MinBy(p => p.Char);
        var maxEnd   = vRanges.Select(r => r!.Value.End  ).MaxBy(p => p.Char);

        return From(minStart, maxEnd);
    }
    public static SourceRange Combine(params SourceRange?[] ranges)
        => Combine((IEnumerable<SourceRange?>)ranges);
    public static SourceRange CombineFrom(IEnumerable<ISourceRanged?> rangeds)
        => Combine(rangeds.Select(r => r?.Range));
    public static SourceRange CombineFrom(params ISourceRanged?[] rangeds)
        => CombineFrom((IEnumerable<ISourceRanged?>)rangeds);
}
