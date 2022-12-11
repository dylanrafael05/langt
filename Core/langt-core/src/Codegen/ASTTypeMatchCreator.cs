using Langt.AST;

namespace Langt.Codegen;

public record struct ASTTypeMatchCreator(ITransformer? Transformer, LangtType? DownflowType)
{
    public void ApplyTo(ASTNode node, CodeGenerator generator)
    {
        if(DownflowType is not null) node.AcceptDownflow(DownflowType, generator);
        if(Transformer  is not null) node.ApplyTransform(Transformer);
    }
}
