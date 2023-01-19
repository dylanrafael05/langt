using System.Diagnostics.CodeAnalysis;

namespace Results.Interfaces;

public interface IResultMonoid : IResultMetadata
{
    bool TryFold(IResultMonoid other, [NotNullWhen(true)] out IResultMonoid? result);
}
public interface IResultMonoid<Self> : IResultMonoid where Self : IResultMonoid<Self>
{
    static abstract Self Identity {get;}
}