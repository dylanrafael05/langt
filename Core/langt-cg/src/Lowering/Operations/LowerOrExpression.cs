using Langt.AST;
using Langt.Structure;

namespace Langt.CG.Lowering;

public struct LowerOrExpression : ILowerImplementation<BoundOrExpression>
{
    public void LowerImpl(CodeGenerator cg, BoundOrExpression node)
    {
        cg.Lower(node.Left);
        var l = cg.PopValue(node.DebugSourceName);

        var start   = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.Operator.Range.CharStart+".or.start");
        var onFalse = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.Operator.Range.CharStart+".or.false");
        var end     = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.Operator.Range.CharStart+".or.end");

        cg.Builder.PositionAtEnd(start);

            cg.Builder.BuildSelect(l, end.AsValue(), onFalse.AsValue());

        cg.Builder.PositionAtEnd(onFalse);

            cg.Lower(node.Right);
            var r = cg.PopValue(node.DebugSourceName);
            
            var real = cg.Builder.BuildOr(l.LLVM, r.LLVM, "or");

        cg.Builder.PositionAtEnd(end);
            
            var phi = cg.Builder.BuildPhi(cg.Binder.Get(LangtType.Bool));
            phi.AddIncoming(new[] {l.LLVM}, new[] {start},   1);
            phi.AddIncoming(new[] {real},   new[] {onFalse}, 1);

            cg.PushValue
            (
                LangtType.Bool,
                phi,
                node.DebugSourceName
            );
    }
}