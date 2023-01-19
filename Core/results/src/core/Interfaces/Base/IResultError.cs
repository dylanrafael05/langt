namespace Results.Interfaces;

public interface IResultError 
{
    IResultMetadata? TryDemote();
}
