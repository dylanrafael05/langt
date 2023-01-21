namespace Langt.Structure;

public class LangtStructureType : LangtResolvableType
{
    public LangtStructureType(string name, IScope scope) : base(name, scope)
    {}

    public IList<LangtStructureField> Fields {get; init;} = new List<LangtStructureField>();
    public bool TryResolveField(string name, out LangtStructureField? field, out int index) 
    {
        // Invariant
        Expect.That(Fields.DistinctBy(f => f.Name).SequenceEqual(Fields), "Structs cannot have duplicate fields");

        var f = Fields.Indexed().Where(f => f.Value.Name == name).ToArray();

        if(f.Length is 0 or > 1)
        {
            field = null;
            index = -1;
            return false;
        }

        field = f.First().Value;
        index = f.First().Index;
        return true;
    }
    public bool HasField(string name) => Fields.Count(f => f.Name == name) is 1;

    public override LLVMTypeRef Lower(Context context)
    {
        var s = context.Context.CreateNamedStruct(Context.LangtIdentifierPrepend + FullName);
        s.StructSetBody(Fields.Select(f => context.LowerType(f.Type)).ToArray(), false);

        return s;
    }
}