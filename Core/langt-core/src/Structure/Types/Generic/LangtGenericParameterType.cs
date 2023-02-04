using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtGenericParameterType : LangtResolvableType
{
    public LangtGenericParameterType(string name, IScope scope) : base(name, scope)
    {}
}
