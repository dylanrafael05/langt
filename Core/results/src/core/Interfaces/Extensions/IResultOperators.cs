namespace Results.Interfaces;

public interface IResultOperators<Self> where Self : IResultOperators<Self>, IResultlike
{
    public static abstract bool operator !     (Self self);
    public static abstract bool operator true  (Self self);
    public static abstract bool operator false (Self self);
}
