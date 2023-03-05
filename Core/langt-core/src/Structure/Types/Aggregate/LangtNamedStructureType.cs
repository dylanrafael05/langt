using Langt.AST;
using Langt.Message;

namespace Langt.Structure;

public class LangtNamedStructureType : LangtResolvableType, IStructureType
{
    public LangtNamedStructureType(string name, FieldSymbol[] fieldSymbols, IScope scope, IScope typeScope, IReadOnlyList<LangtType> genericParameters) : base(name, scope)
    {
        GenericParameters = genericParameters;
        TypeScope = typeScope;
        fieldDict = new();

        this.fieldSymbols = fieldSymbols;
    }

    public IScope TypeScope {get;}
    private readonly FieldSymbol[] fieldSymbols;

    public IEnumerable<string> FieldNames => fieldDict.Keys;
    private readonly Dictionary<string, LangtStructureField> fieldDict;

    public virtual bool ResolveField(string name, out LangtStructureField field) 
    {
        if(!fieldDict.ContainsKey(name))
        {
            field = default;
            return false;
        }

        field = fieldDict[name];
        return true;
    }
    public bool HasField(string name) => fieldDict.ContainsKey(name);

    public override bool? TestAgainstFloating(Func<LangtType, bool?> pred)
        => pred(this) ?? this.Fields().FloatingAny(f => f.Type.TestAgainstFloating(pred));

    public override bool Equals(LangtType? other)
        => other is not null
        && other.IsStructure
        && this.Fields().SequenceEqual(other.Structure.Fields());

    protected override Result CompleteInternal(Context ctx)
    {
        var builder = ResultBuilder.Empty();

        foreach(var fs in fieldSymbols)
        {
            if(fieldDict.ContainsKey(fs.Name))
            {
                builder.AddDgnError(Messages.Get("field-redefine", this, fs.Name), fs.Range);
                continue;
            }
            
            var tyRes = fs.Type.Unravel(ctx);
            builder.AddData(tyRes);

            var ty = tyRes.Or(Error);

            if(ty.Stores(this))
            {
                builder.AddDgnError(Messages.Get("field-cyclic", this, fs.Name), fs.Range);
            }

            fieldDict.Add(fs.Name, new LangtStructureField(fs.Name, ty, fieldDict.Count));
        }

        return builder.Build();
    }

    public override IReadOnlyList<LangtType> GenericParameters {get;}
    public override bool IsConstructed => GenericParameters.Count == 0;
}
