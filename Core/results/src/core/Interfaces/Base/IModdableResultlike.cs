namespace Results.Interfaces;

public interface IModdableResultlike<Self>
    where Self : IModdableResultlike<Self>
{
    Self WithErrors(IEnumerable<IResultError> errors);
    Self WithMetadata(IEnumerable<IResultMetadata> metadata);
    
    Self ClearErrors();
    Self ClearMetadata();
}
