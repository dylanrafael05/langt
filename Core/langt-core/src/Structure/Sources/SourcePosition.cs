namespace Langt;

/// <summary>
/// Represents a single character of a source.
/// </summary>
public readonly record struct SourcePosition(int Char, int Line, int Column, Source Source)
{
    public override string ToString()
        => $"{Source.Name}:line {Line}";
}
