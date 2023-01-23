using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerEmpty : ILowerImplementation<BoundEmpty>
{
    public void LowerImpl(CodeGenerator cg, BoundEmpty node)
    {}
}