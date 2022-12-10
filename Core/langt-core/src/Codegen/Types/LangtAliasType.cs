namespace Langt.Codegen;

public record LangtAliasType(string Name) : LangtType(Name)
{
    public override LangtType? AliasBaseType => basetype;
    private LangtType? basetype;

    public void SetBase(LangtType? basetype)
        => this.basetype = basetype;
    public override LLVMTypeRef Lower(CodeGenerator context)
        => basetype?.Lower(context) ?? throw new Exception("Uninitialized or error alias type");
}
