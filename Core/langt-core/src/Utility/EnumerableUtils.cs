using System.Collections;

namespace Langt.Utility;

public static class EnumerableUtils
{
    public static IEnumerable<(T, T)> Choose<T>(this IEnumerable<T> e, IEnumerable<T> other)
    {
        foreach(var x in e) 
        {
            foreach(var y in other) 
            {
                yield return (x, y);
            }
        }
    }
    public static IEnumerable<(T, T)> ChooseSelf<T>(this IEnumerable<T> e)
        => e.Choose(e);

    public static IEnumerable<(int index, T value)> Indexed<T>(this IEnumerable<T> e) 
        => e.Select((t, i) => (i, t));
}
