namespace Langt;

/// <summary>
/// Represents a single character of a source.
/// </summary>
public readonly record struct SourcePosition(int Char, int Line, int Column, Source Source)
{
    public override string ToString()
        => $"{Source.Name}:line {Line}";
    
    public static bool operator==(SourcePosition a, Position b) 
        => a.Char == b.Char;
    public static bool operator!=(SourcePosition a, Position b) 
        => a.Char != b.Char;

    public static bool operator==(Position a, SourcePosition b) 
        => b == a;
    public static bool operator!=(Position a, SourcePosition b) 
        => b != a;
}

public readonly record struct Position(int Char, int Line, int Column) 
{
    public override string ToString()
        => $"line {Line}";
}
