namespace Langt.Structure;

// TODO: use this class to replace LValue
public class LangtReferenceType : LangtTypeWithElement
{
    private LangtReferenceType(LangtType ptrType) : base(ptrType)
    {}

    protected override string ModifyName(string nameIn)
        => "&" + nameIn;

    public override LLVMTypeRef Lower(Context context)
        => Ptr.Lower(context);

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
}