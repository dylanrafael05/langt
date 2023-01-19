namespace Results.Interfaces;

public interface IModdable<Self>
    where Self : IModdable<Self>
{
    Self WithErrors(IEnumerable<IResultError> errors);
    Self WithMetadata(IEnumerable<IResultMetadata> metadata);
    
    Self ClearErrors();
    Self ClearMetadata();
}
