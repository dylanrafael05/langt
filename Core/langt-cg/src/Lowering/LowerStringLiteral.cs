using Langt.AST;

namespace Langt.CG.Lowering;

public struct LowerStringLiteral : ILowerImplementation<BoundStringLiteral>
{
    public void LowerImpl(CodeGenerator cg, BoundStringLiteral node)
    {
        var res = cg.Builder.BuildGlobalStringPtr(node.Value, "str");

        cg.PushValue( 
            node.Type,
            res,
            node.DebugSourceName
        );
    }
}