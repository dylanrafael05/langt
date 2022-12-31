namespace Results;

public interface IResultlike<Self> : IResultlike where Self : IResultlike<Self>
{
    public abstract static bool operator !(Self self);
    public abstract static bool operator true(Self self);
    public abstract static bool operator false(Self self);
}
