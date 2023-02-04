using System.Diagnostics.CodeAnalysis;

namespace Langt.Structure;

public class LangtPointerType : LangtTypeWithElement
{
    private LangtPointerType(LangtType ptrType) : base(ptrType)
    {}

    protected override string ModifyName(string nameIn)
        => "*" + nameIn;

    public static Result<LangtPointerType> Create(LangtType elem, SourceRange range = default)
    {
        if(elem.IsReference)
        {
            return Result.Error<LangtPointerType>(Diagnostic.Error("Cannot create a pointer to a reference", range));
        }
        
        return CreateElementType<LangtPointerType>(elem => new(elem), elem, range);
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
        && other!.IsPointer;
}
