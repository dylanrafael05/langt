namespace Results;

public interface IMutableResultlike<Self> : IResultlike<Self> where Self : IMutableResultlike<Self>
{
    Self WithErrors(IEnumerable<IResultError> errors);
    Self WithMetadata(IEnumerable<IResultMetadata> metadata);
    
    Self ExcludingErrors(IEnumerable<IResultError> errors);
    Self ExcludingMetadata(IEnumerable<IResultMetadata> metadata);
}
