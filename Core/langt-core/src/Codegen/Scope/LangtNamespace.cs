namespace Langt.Codegen;

public class LangtNamespace : LangtScope, INamedScoped
{
    public string Name {get; init;}
    string INamed.DisplayName => Name;

    public LangtNamespace(string name) 
    {
        Name = name;
    }
}
