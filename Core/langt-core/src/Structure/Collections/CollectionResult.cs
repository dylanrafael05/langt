namespace Langt.Structure.Collections;

public static class CollectionResult
{
    public static T? Nullable<T>(this CollectionResult<T> result) where T : struct 
        => result.Exists 
            ? result.Value 
            : null;
}

public record struct CollectionResult<T>(bool Exists, T? Value)
{
    public static CollectionResult<T> Void => new(false, default);
    public static implicit operator CollectionResult<T>(T? t) => new(true, t);
    public static implicit operator T?(CollectionResult<T> r) => r.Value;

    public override string ToString()
        => Exists 
            ? Value?.ToString() ?? "null"
            : "(non-existant)";
}
