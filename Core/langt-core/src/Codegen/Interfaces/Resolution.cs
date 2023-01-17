namespace Langt.Codegen;

// TODO: continue generalization of resolutions to remove previous interfaces
public abstract class Resolution : IResolution
{   
    public abstract string Name {get;}
    public virtual string DisplayName => Name;
    public virtual string FullName => IResolution.GetFullNameDefault(this);

    public IScope HoldingScope {get;}
    
    public SourceRange? DefinitionRange {get; init;}
    public string? Documentation {get; init;}

    public Resolution(IScope holdingScope) 
    {
        HoldingScope = holdingScope;
    }
}

public class ProxyResolution<T> : IProxyResolution<T> where T : INamed
{
    public ProxyResolution(T inner, IScope holdingScope) 
    {
        Inner = inner;
        HoldingScope = holdingScope;
    }

    public T Inner {get;}

    public IScope HoldingScope {get;}

    public SourceRange? DefinitionRange {get; init;}
    public string? Documentation {get; init;}

    public string Name => Inner.Name;
    public string DisplayName => Inner.DisplayName;
    public string FullName => Inner.FullName;
}
