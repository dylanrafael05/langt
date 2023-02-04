namespace Langt.Structure;

public static class StructureTypeExtensions
{
    public static IEnumerable<LangtStructureField> Fields(this IStructureType ty) 
    {
        foreach(var name in ty.FieldNames)
        {
            if(ty.ResolveField(name, out var f))
            {
                yield return f;
            }
        }
    }
}