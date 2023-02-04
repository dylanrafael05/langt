namespace Langt.Structure;

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

        foreach(var o in options)
        {
            Expect.That(o.IsConstructed, "Option types must contain only constructed types");
        }

        return Result.Success(new LangtOptionType(options));
    }

    public override bool Equals(LangtType? other)
        => other is not null
        && other.IsOption
        && OptionTypes.SetEquals(other.OptionTypes);

    public override LangtType ReplaceGeneric(LangtType gen, LangtType rep)
    {
        if(OptionTypes.Contains(gen))
        {
            var newTypes = new HashSet<LangtType>();

            foreach(var optTy in OptionTypes)
            {
                var k = optTy.ReplaceGeneric(gen, rep);
                newTypes.Add(k);
            }

            return Create(newTypes).Expect();
        }

        return this;
    }
    
    public override bool Contains(LangtType ty)
        => OptionTypes.Any(t => t.Contains(ty)) || base.Contains(ty);
}