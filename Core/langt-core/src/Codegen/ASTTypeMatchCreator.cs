using Langt.AST;

namespace Langt.Codegen;

public record struct ASTTypeMatchCreator(ITransformer? Transformer, LangtType DownflowType)
{
    public void ApplyTo(ASTNode node, TypeCheckState state)
    {
        if(DownflowType is not null) node.TargetTypeCheck(state, DownflowType);
        if(Transformer is not null) node.ApplyTransform(Transformer);
    }

    
}
