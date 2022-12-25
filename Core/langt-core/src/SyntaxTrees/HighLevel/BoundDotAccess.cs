using Langt.Codegen;

namespace Langt.AST;

public record BoundDotAccess(DotAccess SourceNode, BoundASTNode Left) : BoundASTWrapper(SourceNode)
{
    public LangtStructureField? Field {get; set;}
    public int? FieldIndex {get; set;}

    public override bool IsLValue => true;

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(HasResolution) return;

        Left.Lower(lowerer);

        var s = lowerer.PopValue(DebugSourceName);

        lowerer.PushValue( 
            RawExpressionType,
            lowerer.Builder.BuildStructGEP2(
                lowerer.LowerType(s.Type.PointeeType!), 
                s.LLVM,
                (uint)FieldIndex!.Value,
                s.Type.PointeeType!.Name + "." + Field!.Name
            ),
            DebugSourceName
        );
    }
}
