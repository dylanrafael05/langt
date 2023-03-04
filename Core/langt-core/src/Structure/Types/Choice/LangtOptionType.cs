using Langt.AST;

namespace Langt.Structure;

public class OptionTypeSymbol : Symbol<LangtType>
{
    public required IEnumerable<ISymbol<LangtType>> OptionSymbols {get; init;}

    public override Result<LangtType> Unravel(Context ctx)
    {
        var optionSymbols = OptionSymbols.ToList();

        var options = new HashSet<LangtType>();
        var builder = ResultBuilder.Empty();

        foreach(var symbol in optionSymbols)
        {
            var res = symbol.Unravel(ctx);
            builder.AddData(res);

            if(!res) continue;

            var ty = res.Value;

            if(options.Contains(ty))
            {
                builder.AddDgnError($"Duplicate type {ty.FullName} in option", symbol.Range);
                continue;
            }

            options.Add(ty);
        }

        return Result.Success<LangtType>(new LangtOptionType(options));
    }
}

public class LangtOptionType : LangtType
{
    public LangtOptionType(IReadOnlySet<LangtType> opts) 
    {
        Expect.That(opts.Count > 2);

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

            if(newTypes.Count == 1) return newTypes.First();

            return new LangtOptionType(newTypes);
        }

        return this;
    }
    
    public override bool? TestAgainstFloating(Func<LangtType, bool?> pred)
        => pred(this) ?? OptionTypes.FloatingAny(t => t.TestAgainstFloating(pred));
}