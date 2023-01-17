using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;

public class LangtPointerType : LangtType
{
    private LangtPointerType(LangtType ptrType) 
    {
        PointeeType = ptrType;
    }

    [NotNull] public override LangtType? PointeeType {get;}

    public override string Name         => "*" + PointeeType.Name;
    public override string DisplayName  => "*" + PointeeType.DisplayName;
    public override string FullName     => "*" + PointeeType.FullName;

    public override LLVMTypeRef Lower(CodeGenerator context)
        => Ptr.Lower(context);

    public static Result<LangtPointerType> Create(LangtType elem, SourceRange range = default)
    {
        if(elem == None)
        {
            return Result.Error<LangtPointerType>(Diagnostic.Error($"Cannot have a pointer to none", range));
        }

        return Result.Success<LangtPointerType>(new(elem));
    }
}