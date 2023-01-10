using Langt.Codegen;

namespace Langt.AST;

public record BoundStaticAccess(ASTNode Source, BoundASTNode Left, ASTNode Right) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left};

    public BoundStaticAccess(ASTNode Source, BoundASTNode Left, ASTNode Right, INamedScoped Resolution) : this(Source, Left, Right)
    {
        HasResolution = true;
        this.Resolution = Resolution;
    }
}

public record BoundStructFieldAccess(DotAccess SourceNode, BoundASTNode Left) : BoundASTNode(SourceNode)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left};

    public LangtStructureField? Field {get; set;}
    public int? FieldIndex {get; set;}

    public override bool IsLValue => true;

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Left.Lower(lowerer);

        var s = lowerer.PopValue(DebugSourceName);

        lowerer.PushValue( 
            RawExpressionType,
            lowerer.Builder.BuildStructGEP2(
                lowerer.LowerType(s.Type.PointeeType!), 
                s.LLVM,
                (uint)FieldIndex!.Value,
                s.Type.PointeeType!.RawName + "." + Field!.Name
            ),
            DebugSourceName
        );
    }
}
