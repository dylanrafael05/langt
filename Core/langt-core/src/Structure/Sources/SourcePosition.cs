namespace Langt;

/// <summary>
/// Represents a single character of a source.
/// </summary>
public readonly record struct SourcePosition(int Char, int Line, int Column, Source Source)
{
    public override string ToString()
        => $"{Source.Name}:line {Line}";
    
    public static bool operator==(SourcePosition a, SimplePosition b) 
        => (a.Line, a.Column) == (b.Line, b.Column);
    public static bool operator!=(SourcePosition a, SimplePosition b) 
        => b == a;

    public static bool operator==(SimplePosition a, SourcePosition b) 
        => b == a;
    public static bool operator!=(SimplePosition a, SourcePosition b) 
        => b != a;
}

public readonly record struct SimplePosition(int Line, int Column) 
{
    public override string ToString()
        => $"line {Line}";
}
