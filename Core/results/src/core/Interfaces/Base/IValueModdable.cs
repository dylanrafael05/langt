namespace Results.Interfaces;

public interface IValueModdable<Self, T>
    where Self : IValueModdable<Self, T>
{
    Self WithValue(T value);
    Self ExcludingValue();
}