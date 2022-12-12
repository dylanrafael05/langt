using Langt.AST;

namespace Langt.Codegen;

public record struct ASTTypeMatchCreator(ITransformer? Transformer)
{
    public void ApplyTo(ASTNode node, CodeGenerator generator)
    {
        if(Transformer is not null) node.ApplyTransform(Transformer);
    }
}
