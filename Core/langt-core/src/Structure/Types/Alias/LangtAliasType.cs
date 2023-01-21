namespace Langt.Structure;

public class LangtAliasType : LangtResolvableType
{
    [NotNull] public override LangtType? AliasBaseType => baseType;
    private LangtType? baseType;

    public LangtAliasType(string name, IScope scope) : base(name, scope)
    {}

    public void SetBase(LangtType? baseType)
        => this.baseType = baseType;
    public override LLVMTypeRef Lower(Context context)
        => baseType?.Lower(context) ?? throw new Exception("Uninitialized or error alias type");
}
