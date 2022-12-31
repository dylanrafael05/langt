namespace Results;

public interface IResultlike
{
    IEnumerable<IResultError> Errors {get;}
    IEnumerable<IResultMetadata> Metadata {get;}

    bool HasErrors {get;}
    bool HasMetadata {get;}
}
