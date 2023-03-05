using Langt.AST;
using Langt.Message;

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

    protected override Result CompleteInternal(Context ctx)
    {
        var tyRes = baseSymbol.Unravel(ctx);
        if(!tyRes) return tyRes.Drop();

        var ty = tyRes.Value;

        if(ty.Contains(this))
        {
            return Result.Error(Diagnostic.Error(Messages.Get("alias-recursion", this), baseSymbol.Range));
        }

        baseType = ty;
        
        return Result.Success();
    }

    public override bool? TestAgainstFloating(Func<LangtType, bool?> pred)
        => pred(this) ?? baseType.TestAgainstFloating(pred);
}
