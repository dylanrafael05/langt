using Langt.AST;
using Langt.Lexing;

namespace Langt.CG.Lowering;

public struct LowerFunctionDefinition : ILowerImplementation<BoundFunctionDefinition>
{
    public void LowerImpl(CodeGenerator cg, BoundFunctionDefinition node)
    {
        // TODO: reimpl
        if(node.Source.Let.Type is TokenType.Extern) return; //work is already done for us when creating the prototypes!

        cg.Function(node.Function, () =>
        {
            cg.Lower(node.Body);
        });
    }
}
