namespace Langt.Structure;

// TODO: use this class to replace LValue
public class LangtReferenceType : LangtTypeWithElement
{
    private LangtReferenceType(LangtType ptrType) : base(ptrType)
    {}

    protected override string ModifyName(string nameIn)
        => "&" + nameIn;

    public static Result<LangtReferenceType> Create(LangtType elem, SourceRange range = default)
    {
        if(elem.IsReference)
        {
            return Result.Error<LangtReferenceType>(
                Diagnostic.Error(
                    "Cannot create a nested reference type",
                    range
                )
            );
        }

        return CreateElementType<LangtReferenceType>(elem => new(elem), elem, range);
    }
    
    public override LangtType ReplaceGeneric(LangtType gen, LangtType rep)
    {
        if(ElementType.Contains(gen)) 
        {
            var newElem = ElementType.ReplaceGeneric(gen, rep);
            return Create(newElem).Expect();
        }
        
        return this;
    }
    
    public override bool Equals(LangtType? other)
        => base.Equals(other) 
        && other!.IsReference;
}