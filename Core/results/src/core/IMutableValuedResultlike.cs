namespace Results;

public interface IMutableValuedResultlike<Self, T> : IMutableResultlike<Self>, IValued<T> where Self : IMutableValuedResultlike<Self, T>
{
    Self WithValue(T value);
    Self ExcludingValue();
}