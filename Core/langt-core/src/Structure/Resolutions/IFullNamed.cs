namespace Langt.Structure.Resolutions;

public interface IFullNamed : INamed
{
    string DisplayName {get;}
    string FullName {get;}
}
