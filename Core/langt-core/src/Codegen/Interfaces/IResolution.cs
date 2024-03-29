namespace Langt.Codegen;

public interface IResolution : INamed
{
    IScope HoldingScope {get;}
    
    SourceRange? DefinitionRange {get;}
    string? Documentation {get;}

    public static string GetFullNameDefault(IResolution item) 
    {
        var name = item.DisplayName;

        if(item is not IResolution scoped)  
            return name;
        if(scoped.HoldingScope is not INamed holding)     
            return name;

        var upperName = holding.FullName;

        if(string.IsNullOrEmpty(upperName)) return name;

        return upperName + "::" + name;
    }
}
