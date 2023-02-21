using Langt.Structure;
using Langt.Structure.Resolutions;

namespace Langt.AST;

public record BoundVariableReference(BoundASTNode BoundSource, LangtVariable Variable) : BoundASTNode(BoundSource.ASTSource)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {BoundSource};

    public override LangtType Type => Variable.Type;

    public override IResolvable? Resolution => Variable;
    public override bool HasResolution => true;

    public override bool IsAssignable => true;
}
