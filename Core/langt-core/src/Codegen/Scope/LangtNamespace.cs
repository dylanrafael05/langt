namespace Langt.Codegen;

// TODO: handle this! both a scope and resolution
public class LangtNamespace : LangtScope, Resolution
{
    public LangtNamespace(LangtScope scope) : base(scope) 
    {}

    public required string Name {get; init;}
    public string DisplayName => Name;
    public string FullName => Resolution.GetFullName(this);
}
