namespace Results;

public interface IResultError 
{
    IResultMetadata? TryDemote();
}