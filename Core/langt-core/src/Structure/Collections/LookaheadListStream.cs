namespace Langt.Structure;

public abstract class LookaheadListStream<T>
{
    public IReadOnlyList<T> Source {get; protected init;} = new List<T>();
    public int index = 0;

    public bool ReachedEnd => index > Source.Count;
    public CollectionResult<T> Get(int offset) 
        => index + offset is var i 
        && i >= Source.Count || i < 0 
            ? CollectionResult<T>.Void 
            : Source[i];
    
    // Character getters
    public CollectionResult<T> Last => Get(-1);
    public CollectionResult<T> Current => Get(0);
    public CollectionResult<T> Next => Get(1);

    // Helpers
    public virtual void Pass(int count = 1)
    {
        index += count;
    }
    public int PassAll(Predicate<T> pred)
    {
        var count = 0;
        while(Current.Exists && pred(Current!)) 
        {
            count++;
            index++;
        }
        return count;
    }
    public int PassWhile(Func<bool> pred)
    {
        var count = 0;
        while(Current.Exists && pred()) 
        {
            index++;
            count++;
        }
        return count;
    }
}