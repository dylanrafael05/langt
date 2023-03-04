namespace Langt.Structure;

public class ProxyResolution : ImmediateResolvable
{
    [SetsRequiredMembers]
    public ProxyResolution(IScope holdingScope, IFullNamed item)
    {
        Item = item;

        Name = item.Name;
        HoldingScope = holdingScope;
    }
    public IFullNamed Item {get;}
}


