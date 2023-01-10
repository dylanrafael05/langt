namespace Langt.Codegen;

// TODO: continue generalization of resolutions to remove previous interfaces
public abstract class Resolution
{   
    public abstract string Name {get;}
    public virtual string DisplayName => Name;
    public virtual string FullName => GetFullName(this);

    public Resolution(LangtScope holdingScope) 
    {
        HoldingScope = holdingScope;
    }
    
    public LangtScope HoldingScope {get;}
    
    public SourceRange? DefinitionRange {get; init;}
    public string? Documentation {get; init;}

    public static string GetFullName(Resolution? named) 
    {
        if(named is null) return "";

        var name = named.DisplayName;

        if(named is not IScoped scoped)  return name;
        if(!scoped.HoldingScope.HasName) return name;

        var upperName = GetFullName(scoped.HoldingScope);

        if(string.IsNullOrEmpty(upperName)) return name;

        return upperName + "::" + name;
    }
}
