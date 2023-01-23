using Langt.AST;
using Langt.Lexing;
using Langt.Parsing;

namespace Langt.CG.Lowering;

public struct LowerConversion : ILowerImplementation<BoundConversion>
{
    public void LowerImpl(CodeGenerator cg, BoundConversion node)
    {
        cg.Lower(node.Source);
        var pre = cg.PopValue(node.DebugSourceName);

        cg.PushValue
        (
            node.Conversion.Output, 
            cg.Binder.Get(node.Conversion)(pre),
            node.DebugSourceName
        );
    }
}
