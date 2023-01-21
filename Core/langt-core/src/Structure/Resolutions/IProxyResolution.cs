namespace Langt.Structure.Resolutions;

public interface IProxyResolution<T> : IResolution where T : INamed
{
    T Inner {get;}
}