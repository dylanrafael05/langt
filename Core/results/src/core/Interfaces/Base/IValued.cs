namespace Results.Interfaces;

public interface IValued<T>
{
    bool HasValue {get;}
    T Value {get;}
}
