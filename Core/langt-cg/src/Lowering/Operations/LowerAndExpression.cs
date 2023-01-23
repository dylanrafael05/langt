using Langt.AST;
using Langt.Structure;

namespace Langt.CG.Lowering;

public struct LowerAndExpression : ILowerImplementation<BoundAndExpression>
{
    public void LowerImpl(CodeGenerator cg, BoundAndExpression node)
    {
        cg.Lower(node.Left);
        var l = cg.PopValue(node.DebugSourceName);

        var start  = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.Operator.Range.CharStart+".and.start");
        var onTrue = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.Operator.Range.CharStart+".and.true");
        var end    = cg.LLVMContext.AppendBasicBlock(cg.CurrentFunction!, node.Source.Operator.Range.CharStart+".and.end");

        cg.Builder.PositionAtEnd(start);

            cg.Builder.BuildSelect(l, onTrue.AsValue(), end.AsValue());

        cg.Builder.PositionAtEnd(onTrue);

            cg.Lower(node.Right);
            var r = cg.PopValue(node.DebugSourceName);
            
            var real = cg.Builder.BuildAnd(l, r, "and");

        cg.Builder.PositionAtEnd(end);
            
            var phi = cg.Builder.BuildPhi(cg.Binder.Get(LangtType.Bool));
            phi.AddIncoming(new[] {l.LLVM}, new[] {start},  1);
            phi.AddIncoming(new[] {real},   new[] {onTrue}, 1);

            cg.PushValue
            (
                LangtType.Bool,
                phi,
                node.DebugSourceName
            );
    }
}
