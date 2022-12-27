namespace Results;

public interface IResult<Self> : IResult where Self : IResult<Self>
{
    public abstract static bool operator !(Self self);
    public abstract static bool operator true(Self self);
    public abstract static bool operator false(Self self);
}
