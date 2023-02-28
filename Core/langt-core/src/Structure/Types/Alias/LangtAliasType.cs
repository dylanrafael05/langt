using Langt.AST;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public class LangtAliasType : LangtResolvableType
{
    public override LangtType? AliasBaseType => baseType;
    private LangtType baseType;
    private readonly ISymbol<LangtType> baseSymbol;

    public LangtAliasType(string name, IScope scope, ISymbol<LangtType> baseSymbol) : base(name, scope)
    {
        this.baseSymbol = baseSymbol;
        baseType = Error;
    }

    public override Result Complete(Context ctx)
    {
        var tyRes = baseSymbol.Unravel(ctx);
        if(!tyRes) return tyRes.Drop();

        var ty = tyRes.Value;

        if(ty.Contains(this))
        {
            return Result.Error(Diagnostic.Error($"Alias types cannot refer to themselves!", baseSymbol.Range));
        }

        baseType = ty;
        
        return Result.Success();
    }
}
