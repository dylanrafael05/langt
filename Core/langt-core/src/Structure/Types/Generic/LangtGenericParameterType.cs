

namespace Langt.Structure;

public class LangtGenericParameterType : LangtResolvableType
{
    public LangtGenericParameterType(string name, IScope scope) : base(name, scope)
    {}

    protected override Result CompleteInternal(Context ctx)
        => Result.Success();
}
