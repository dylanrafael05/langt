namespace Langt.Codegen;

public interface IScoped 
{
    LangtScope? HoldingScope {get; set;}

    string FullName => ScopedImpl.GetFullName(this);
}
