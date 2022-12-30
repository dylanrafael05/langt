namespace Langt.Codegen;

public interface IScoped 
{
    LangtScope? HoldingScope {get; set;}
}
