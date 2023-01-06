namespace Results;
using Interfaces;

public record ExceptionError(Exception Source) : IResultError
{
    public IResultMetadata? TryDemote() => null;
}
