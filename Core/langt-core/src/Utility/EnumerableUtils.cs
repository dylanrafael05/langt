using System.Collections;
using System.Runtime.InteropServices;

namespace Langt.Utility;

public static class EnumerableExtensions
{
    public static IEnumerable<(T First, T Second)> Choose<T>(this IEnumerable<T> e, IEnumerable<T> other)
    {
        foreach(var x in e) 
        {
            foreach(var y in other) 
            {
                yield return (x, y);
            }
        }
    }
    public static IEnumerable<(T First, T Second)> ChooseSelfUnique<T>(this IEnumerable<T> e)
    {
        var skip = 0;
        foreach(var x in e) 
        {
            foreach(var y in e.Skip(skip)) 
            {
                yield return (x, y);
            }
            skip++;
        }
    }
    public static IEnumerable<(T First, T Second)> ChooseSelf<T>(this IEnumerable<T> e)
        => e.Choose(e);

    public static IEnumerable<(int Index, T Value)> Indexed<T>(this IEnumerable<T> e) 
        => e.Select((t, i) => (i, t));

    public static IEnumerable<T> Without<T>(this IEnumerable<T> t, T value)
        => t.Where(x => x?.Equals(value) ?? (value is null));

    public static IEnumerable<T> OrEmpty<T>(IEnumerable<T>? t) 
        => t ?? Array.Empty<T>();

    public static ref V? GetOrAddDefaultRef<K, V>(this Dictionary<K, V> d, K key, out bool exists) where K : notnull
        => ref CollectionsMarshal.GetValueRefOrAddDefault(d, key, out exists);

    [Flags]
    private enum MergeSide 
    {
        None  = 0b00,
        Left  = 0b01,
        Right = 0b10,
        Both  = 0b11
    }

    public static List<T> MergeSorted<T>(List<T> left, List<T> right, IComparer<T> comparer)
    {
        return left.Concat(right).Order(comparer).ToList();

        // TODO: REINSTATE
        /*
        var totalCount = left.Count + right.Count;
        var res = new List<T>(totalCount);

        var leftidx  = 0;
        var rightidx = 0;

        for(var repeat = 0; repeat < totalCount; repeat++)
        {
            var side = MergeSide.None;

            if(leftidx >= left.Count)
                side |= MergeSide.Right;
            
            if(rightidx >= right.Count)
                side |= MergeSide.Left;
            
            Expect.That(side is not MergeSide.None);

            leftidx  += (side & MergeSide.Left)  == 0 ? 0 : 1;
            rightidx += (side & MergeSide.Right) == 0 ? 0 : 1;

            if(comparer.Compare(left[leftidx], right[rightidx]) >= 0)
            {
                res.Add(left[leftidx]);
            }
            else
            {
                res.Add(right[rightidx]);
            }
        }

        return res;
        */
    }
}
