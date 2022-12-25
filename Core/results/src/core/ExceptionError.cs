namespace Results;

public record ExceptionError(Exception Source) : IResultError
{
    public IResultMetadata? TryDemote() => null;
}
