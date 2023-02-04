namespace Langt.Structure;

public abstract class LangtTypeWithElement : LangtType
{
    protected LangtTypeWithElement(LangtType elemTy) 
    {
        ElementType = elemTy;
    }

    protected abstract string ModifyName(string nameIn);

    [NotNull] public override LangtType? ElementType {get;}

    public override string Name         => ModifyName(ElementType.Name);
    public override string DisplayName  => ModifyName(ElementType.DisplayName);
    public override string FullName     => ModifyName(ElementType.FullName);

    public override bool Equals(LangtType? other)
        => other is not null
        && ElementType == other.ElementType;

    public override bool Contains(LangtType ty)
        => ElementType.Contains(ty) || base.Contains(ty);

    protected static Result<T> CreateElementType<T>(Func<LangtType, T> constructor, LangtType elem, SourceRange range = default) where T : LangtTypeWithElement
    {
        Expect.That(elem.IsConstructed, "Element types must contain only constructed types");

        if(elem == None)
        {
            return Result.Error<T>(Diagnostic.Error($"Cannot have a pointer to none", range));
        }

        return Result.Success(constructor(elem));
    }
}
