using Langt.Codegen;
using Langt.Utility;

namespace Langt.AST;

public record UsingDeclaration(ASTToken Using, ASTNamespace Identifier) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Using, Identifier};

    public override Result<BoundASTNode> Bind(ASTPassState state)
        => new BoundASTWrapper(this);

    public override Result HandleDefinitions(ASTPassState state)
    {
        var ns = Identifier.Resolve(state);

        if(ns is null)
        {
            Result.Failure(("Cannot have a 'using' declaration which uses a non-namespace", Range));
        }

        state.CG.CurrentFile!.Scope.IncludedNamespaces.Add(ns);

        return Result.Success;
    }
}