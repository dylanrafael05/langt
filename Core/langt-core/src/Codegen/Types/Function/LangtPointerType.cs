using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;

public class LangtPointerType : LangtType
{
    private LangtPointerType() {}
    private LangtType pointeeType = null!;

    public override LangtType? PointeeType => pointeeType;
    public override string RawName            => "*" + PointeeType!.RawName;
    public override string Name     => "*" + PointeeType!.Name;
    public override string FullName        => "*" + PointeeType!.FullName;

    public override LLVMTypeRef Lower(CodeGenerator context)
        => Ptr.Lower(context);

    public static bool TryCreate(LangtType elem, [NotNullWhen(true)] out LangtPointerType? type)
    {
        if(elem == None)
        {
            type = null;
            return false;
        }
        else 
        {
            type = new() {pointeeType = elem};
            return true;
        }
    }
    public static LangtPointerType Create(LangtType elem)
    {
        Expect.That
        (
            TryCreate(elem, out var t), 
            $"{nameof(LangtPointerType)}.{nameof(Create)} expects no errors to occur during type creation!"
        );

        return t!;
    }
}