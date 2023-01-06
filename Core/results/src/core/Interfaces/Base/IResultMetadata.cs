namespace Results.Interfaces;

public interface IResultMetadata
{
    public static IEnumerable<IResultMetadata> MergeMetadataImmediate(IEnumerable<IResultMetadata> a, IEnumerable<IResultMetadata> b)
        => MergeMetadataDeffered(a, b).ToArray();
    public static IEnumerable<IResultMetadata> MergeMetadataDeffered(IEnumerable<IResultMetadata> a, IEnumerable<IResultMetadata> b)
    {
        foreach(var v in a.Where(k => k is not IResultMonoid)) 
            yield return v;

        foreach(var v in b.Where(k => k is not IResultMonoid)) 
            yield return v;

        if(!a.Any(k => k is IResultMonoid) && !b.Any(k => k is IResultMonoid))
            yield break;

        var monoids = a.OfType<IResultMonoid>()
            .Concat(b.OfType<IResultMonoid>())
            .ToHashSet();

        while(monoids.Count > 0)
        {
            var newMonoid = monoids.First();
            monoids.Remove(newMonoid);

            foreach(var m in monoids.ToArray())
            {
                if(newMonoid.TryFold(m, out var res))
                {
                    monoids.Remove(m);
                    newMonoid = res;
                }
            }

            yield return newMonoid;
        }
    }
}