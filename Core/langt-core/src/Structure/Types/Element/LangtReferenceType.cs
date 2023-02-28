using Langt.AST;

namespace Langt.Structure;

public class ReferenceTypeSymbol : Symbol<LangtType>
{
    public required ISymbol<LangtType> ElementType {get; init;}

    public override Result<LangtType> Unravel(Context ctx)
    {
        var tyRes = ElementType.Unravel(ctx);
        if(!tyRes) return tyRes;

        var ty = tyRes.Value;
        
        if(ty.IsReference)
        {
            return Result.Error<LangtType>(
                Diagnostic.Error(
                    "Cannot create a nested reference type",
                    Range
                )
            );
        }

        return Result.Success<LangtType>(new LangtReferenceType(ty));
    }
}

public class LangtReferenceType : LangtTypeWithElement
{
    public LangtReferenceType(LangtType ptrType) : base(ptrType)
    {
        Expect.Not(ptrType.IsReference);
    }

    protected override string ModifyName(string nameIn)
        => "&" + nameIn;
    
    public override LangtType ReplaceGeneric(LangtType gen, LangtType rep)
    {
        if(ElementType.Contains(gen)) 
        {
            var newElem = ElementType.ReplaceGeneric(gen, rep);
            return new LangtReferenceType(newElem);
        }
        
        return this;
    }
    
    public override bool Equals(LangtType? other)
        => base.Equals(other) 
        && other!.IsReference;
}