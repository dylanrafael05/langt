namespace Results;

public interface IResult
{
    bool HasValue {get;}
    bool HasErrors {get;}
    bool HasMetadata {get;}

    object? Value {get;}
    IEnumerable<IResultError> Errors {get;}
    IEnumerable<IResultMetadata> Metadata {get;}
}

// TODO: break this class into two: 'IResultDataHolder' and 'IResult'
// 'IResultDataHolder' would be responsible for errors and would be required to have 
// '!', 'true', 'false', 'HasErrors', 'HasMetadata', 'Errors', and 'Metadata'
// while 'IResult' would be an extension with 'HasValue' and 'Value', and would
// benefit from extension methods solely