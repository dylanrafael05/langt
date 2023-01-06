namespace Langt.Codegen;

public record LangtStructureType(string Name, string Documentation = "") : LangtType(Name, Documentation)
{
    public record struct ResolveFieldResult(ResolveFieldResult.ResultType Type)
    {
        public enum ResultType
        {
            Success,
            NoneFound,
            MoreThanOneFound
        }

        public static bool operator true(ResolveFieldResult t) 
            => t.Type == ResultType.Success;
        public static bool operator false(ResolveFieldResult t) 
            => t.Type != ResultType.Success;

        public static ResolveFieldResult Success => new(ResultType.Success);
        public static ResolveFieldResult NoneFound => new(ResultType.NoneFound);
        public static ResolveFieldResult MoreThanOneFound => new(ResultType.MoreThanOneFound);
    }

    public IList<LangtStructureField> Fields {get; init;} = new List<LangtStructureField>();
    public bool TryResolveField(string name, out LangtStructureField? field, out int index, out ResolveFieldResult result) 
    {
        var f = Fields.Select((j, i) => (item: j, index: i)).Where(f => f.item.Name == name);

        if(f.Count() is 0)
        {
            field = null;
            index = -1;
            result = ResolveFieldResult.NoneFound;
            return false;
        }

        if(f.Count() > 1)
        {
            field = null;
            index = -1;
            result = ResolveFieldResult.MoreThanOneFound;
            return false;
        }

        field = f.First().item;
        index = f.First().index;
        result = ResolveFieldResult.Success;
        return true;
    }
    public bool HasField(string name) => Fields.Count(f => f.Name == name) is 1;

    public override LLVMTypeRef Lower(CodeGenerator context)
    {
        var s = context.LLVMContext.CreateNamedStruct(CodeGenerator.LangtIdentifierPrepend + this.GetFullName());
        s.StructSetBody(Fields.Select(f => context.LowerType(f.Type)).ToArray(), false);

        return s;
    }
}