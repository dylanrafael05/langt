using System.Collections;

namespace Langt.Utility.Collections;

/// <summary>
/// Represents a generic list which is sorted based on some provided comparer.
/// Has the same functionality as a list, except for .Insert() which throws a NotSupportedException.
/// </summary>
public class OrderedList<T> : IList<T>
{
    private List<T> inner;
    private readonly IComparer<T> comparer;

    public OrderedList()
        : this(null, null)
    {}
    public OrderedList(int capacity)
        : this(capacity, null)
    {}
    public OrderedList(IComparer<T> comparer)
        : this(null, comparer)
    {}
    public OrderedList(int? capacity, IComparer<T>? comparer)
    {
        inner = capacity is int x 
            ? new(x) 
            : new();
        this.comparer = comparer ?? Comparer<T>.Default;
    }
    public OrderedList(OrderedList<T> other) 
    {
        inner = new(other);
        comparer = other.comparer;
    }

    public T this[int index]
    {
        get => inner[index];
        set => inner[index] = value;
    }

    public int Count => inner.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        if(Count == 0) inner.Add(item);
        else if(comparer.Compare(inner[0],  item) <  0) inner.Insert(0, item);
        else if(comparer.Compare(inner[^1], item) >= 0) inner.Add(item);
        else 
        {
            var idx = inner.BinarySearch(item, comparer);

            if(idx < 0) idx = ~idx;

            inner.Insert(idx, item);
        }
    }

    public void AddRange(IEnumerable<T> other) 
    {
        if(other is OrderedList<T> ordered && ordered.comparer == this.comparer)
        {
            inner = EnumerableExtensions.MergeSorted(inner, ordered.inner, comparer);
        }
        else 
        {
            foreach(var value in other) 
            {
                Add(value);
            }
        }
    }

    public OrderedList<T> Merge(IEnumerable<T> other)
    {
        var n = new OrderedList<T>(this);
        n.AddRange(other);

        return n;
    }

    public void Clear()
        => inner.Clear();

    public bool Contains(T item)
        => inner.Contains(item);

    public void CopyTo(T[] array, int arrayIndex)
        => inner.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator()
        => inner.GetEnumerator();

    public int IndexOf(T item)
        => inner.BinarySearch(item, comparer);
    public int IndexGreaterThan(T item) 
    {
        var idx = IndexOf(item);
        return idx < 0
            ? ~idx
            : idx
        ;
    }

    public void Insert(int index, T item)
        => throw new NotSupportedException("Cannot insert into an ordered list");

    public bool Remove(T item)
    {
        var idx = IndexOf(item);
        if(idx < 0) return false;

        inner.RemoveAt(idx);
        return true;
    }
    public void RemoveAt(int index)
        => inner.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator()
        => inner.GetEnumerator();
}
