namespace Langt.Codegen;

public class LangtOptionType : LangtType
{
    private LangtOptionType(IReadOnlySet<LangtType> opts) 
    {
        // Generate sorted type map
        var optsSort = opts.OrderBy(t => t.Name).Indexed().ToList();

        OptTypes   = opts;
        OptTypeMap = optsSort.ToDictionary(o => o.Value, o => o.Index);
    }

    public override string Name        => string.Join(" | ", OptTypeMap.Keys.Select(t => t.Name));
    public override string DisplayName => string.Join(" | ", OptTypeMap.Keys.Select(t => t.DisplayName));
    public override string FullName    => string.Join(" | ", OptTypeMap.Keys.Select(t => t.FullName));

    private IReadOnlySet<LangtType> OptTypes {get; init;}
    private IReadOnlyDictionary<LangtType, int> OptTypeMap {get; init;}

    [NotNull] public override IReadOnlySet<LangtType>? OptionTypes => OptTypes;
    [NotNull] public override IReadOnlyDictionary<LangtType, int>? OptionTypeMap => OptTypeMap;

    public const uint TagLocation = 1;

    // TODO: ensure that pointers to option types are also valid pointers to their options
    public override LLVMTypeRef Lower(CodeGenerator context)
    {
        var maxSize = OptionTypes.Max(context.Sizeof);

        var elems = new List<LLVMTypeRef>
        {
            LLVMTypeRef.CreateArray(LLVMTypeRef.Int8, (uint)maxSize),
            LLVMTypeRef.Int8 // Tag
        };

        var s = context.Context.CreateNamedStruct(FullName);
        s.StructSetBody(elems.ToArray(), false);

        return s;
    }

    public static Result<LangtOptionType> Create(IReadOnlySet<LangtType> options, SourceRange range = default)
    {
        if(options.Count == 0)
        {
            return Result.Error<LangtOptionType>(Diagnostic.Error("Cannot create an option type with no options", range));
        }

        if(options.Count == 1)
        {
            return Result.Error<LangtOptionType>(Diagnostic.Error("Cannot create an option type with only one option", range));
        }

        return Result.Success(new LangtOptionType(options));
    }
}