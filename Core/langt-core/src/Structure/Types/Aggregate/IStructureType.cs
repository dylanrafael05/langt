

namespace Langt.Structure;

public interface IStructureType : IFullNamed
{
    bool ResolveField(string name, out LangtStructureField field);
    bool HasField(string name);

    IEnumerable<string> FieldNames {get;}
    IReadOnlyList<LangtType> GenericParameters {get;}

    IScope? TypeScope {get;}
}
