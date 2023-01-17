namespace Langt.Codegen;

public interface IProxyResolution<T> : IResolution where T : INamed
{
    T Inner {get;}
}