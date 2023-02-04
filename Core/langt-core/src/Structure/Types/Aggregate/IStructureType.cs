using Langt.Structure.Resolutions;

namespace Langt.Structure;

public interface IStructureType : INamed
{
    bool ResolveField(string name, out LangtStructureField field);
    bool HasField(string name);

    IEnumerable<string> FieldNames {get;}
    IReadOnlyList<LangtType> GenericParameters {get;}

    IScope? TypeScope {get;}

    void AddField(string name, LangtType ty);
}
