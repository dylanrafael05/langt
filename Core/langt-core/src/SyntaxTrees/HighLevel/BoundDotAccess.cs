using Langt.Codegen;

namespace Langt.AST;

public record BoundStaticAccess(ASTNode Source, BoundASTNode Left, ASTNode Right) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Left};

    public BoundStaticAccess(ASTNode Source, BoundASTNode Left, ASTNode Right, IResolution Resolution) : this(Source, Left, Right)
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

    public override LangtType Type => LangtReferenceType.Create(Field!.Type).Expect();

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Left.Lower(lowerer);

        var s = lowerer.PopValue(DebugSourceName);

        lowerer.PushValue( 
            Type,
            lowerer.Builder.BuildStructGEP2(
                lowerer.LowerType(s.Type.ElementType!), 
                s.LLVM,
                (uint)FieldIndex!.Value,
                s.Type.ElementType!.Name + "." + Field!.Name
            ),
            DebugSourceName
        );
    }
}
