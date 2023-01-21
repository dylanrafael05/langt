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

    protected static Result<T> CreateElementType<T>(Func<LangtType, T> constructor, LangtType elem, SourceRange range = default) where T : LangtTypeWithElement
    {
        if(elem == None)
        {
            return Result.Error<T>(Diagnostic.Error($"Cannot have a pointer to none", range));
        }

        return Result.Success(constructor(elem));
    }
}
