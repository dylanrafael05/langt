using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Resolutions;
using Langt.Structure.Visitors;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

// TODO: MAJOR: prevent "cascading" state assignment of pointer reading; reset non-err state variables automatically?

public record BoundVariableReference(BoundASTNode BoundSource, LangtVariable Variable) : BoundASTNode(BoundSource.ASTSource)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {BoundSource};

    public override LangtType Type => LangtReferenceType.Create(Variable.Type).Expect();

    public override IResolution? Resolution => Variable;
    public override bool HasResolution => true;
}

public record Identifier(ASTToken Tok) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.VisitNoDepth(Tok);

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        var resolution = state.CTX.ResolutionScope.Resolve(Tok.ContentStr, Range);
        builder.AddData(resolution);
        if(!resolution) return builder.BuildError<BoundASTNode>();

        builder.AddStaticReference(Range, resolution.Value);

        return builder.Build<BoundASTNode>
        (
            new BoundEmpty(this)
            {
                HasResolution = true,
                Resolution = resolution.Value
            }
        );
    }
}
