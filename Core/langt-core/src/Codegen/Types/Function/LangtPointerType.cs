using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;

public record LangtPointerType(LangtType PointeeTypeField) : LangtType("*" + PointeeTypeField.Name)
{
    [NotNull] public override LangtType? PointeeType => PointeeTypeField;
    public override LLVMTypeRef Lower(CodeGenerator context)
        => Utility.LLVMUtil.CreatePointerType(context.LLVMContext, 0);
}