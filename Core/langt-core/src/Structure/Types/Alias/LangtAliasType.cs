using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtAliasType : LangtResolvableType
{
    public override LangtType? AliasBaseType => baseType;
    private LangtType? baseType;

    public LangtAliasType(string name, IScope scope) : base(name, scope)
    {}

    public void SetBase(LangtType? baseType)
    {
        this.baseType = baseType;

        if(baseType is not null)
            Expect.That(baseType.IsConstructed, "Alias types can only contain constructed types");
    }
}
