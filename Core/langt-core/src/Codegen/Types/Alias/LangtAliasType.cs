namespace Langt.Codegen;

public class LangtAliasType : LangtResolvableType
{
    public override LangtType? AliasBaseType => baseType;
    private LangtType? baseType;

    public LangtAliasType(string name, IScope scope) : base(name, scope)
    {}

    public void SetBase(LangtType? baseType)
        => this.baseType = baseType;
    public override LLVMTypeRef Lower(CodeGenerator context)
        => baseType?.Lower(context) ?? throw new Exception("Uninitialized or error alias type");
}
