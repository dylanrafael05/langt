using System.Diagnostics.CodeAnalysis;
using Langt.AST;

namespace Langt.Structure;

public class PointerTypeSymbol : Symbol<LangtType>
{
    public required ISymbol<LangtType> ElementType {get; init;}

    public override Result<LangtType> Unravel(Context ctx)
    {
        var tyRes = ElementType.Unravel(ctx);
        if(!tyRes) return tyRes;

        var ty = tyRes.Value;

        if(ty.IsReference)
        {
            return Result.Error<LangtType>(Diagnostic.Error("Cannot create a pointer to a reference", Range));
        }
        
        return Result.Success<LangtType>(new LangtPointerType(ty));
    }
}

public class LangtPointerType : LangtTypeWithElement
{
    public LangtPointerType(LangtType ptrType) : base(ptrType)
    {
        Expect.Not(ptrType.IsReference);
    }

    protected override string ModifyName(string nameIn)
        => "*" + nameIn;

    public override LangtType ReplaceGeneric(LangtType gen, LangtType rep)
    {
        if(ElementType.Contains(gen)) 
        {
            var newElem = ElementType.ReplaceGeneric(gen, rep);
            return new LangtPointerType(newElem);
        }
        
        return this;
    }

    public override bool Equals(LangtType? other)
        => base.Equals(other) 
        && other!.IsPointer;
}
