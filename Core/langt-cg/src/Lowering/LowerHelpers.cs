using Langt.AST;
using Langt.CG.Structure;
using Langt.Structure;

namespace Langt.CG.Lowering;

public static class LowerHelpers
{
    public static LangtValue BuildStructGEP(CodeGenerator cg, BoundASTNode value, int fieldIndex, LangtStructureField field, string debugSourceName)
    {
        cg.Lower(value);

        var s = cg.PopValue(debugSourceName);

        return new LangtValue
        (
            field.Type,
            cg.Builder.BuildStructGEP2
            (
                cg.Binder.Get(s.Type.ElementType!), 
                s.LLVM,
                (uint)fieldIndex,
                s.Type.ElementType!.Name + "." + field.Name
            )
        );
    }
}