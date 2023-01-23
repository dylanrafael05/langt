using Langt.Structure;
using Langt.Utility;

namespace Langt.AST;

public record UsingDeclaration(ASTToken Using, ASTNamespace Identifier) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Using, Identifier};

    public override Result HandleDefinitions(ASTPassState state)
    {
        var ns = Identifier.Resolve(state);

        if(ns)
        {
            state.CTX.CurrentFile!.IncludeNamespace(ns.Value);
        }

        return ns.Drop();
    }
}