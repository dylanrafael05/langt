using Langt.Structure;
using Langt.Utility;

namespace Langt.AST;

public record UsingDeclaration(ASTToken Using, ASTNamespace Identifier) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Using, Identifier};

    public override Result HandleDefinitions(Context ctx)
    {
        var ns = Identifier.Resolve(ctx);

        if(ns)
        {
            ctx.CurrentFile!.IncludeNamespace(ns.Value);
        }

        return ns.Drop();
    }
}