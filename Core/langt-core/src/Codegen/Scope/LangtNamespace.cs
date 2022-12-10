namespace Langt.Codegen;

public class LangtNamespace : LangtScope, INamedScoped
{
    public string Name {get; init;}

    public LangtNamespace(string name) 
    {
        Name = name;
    }
}
