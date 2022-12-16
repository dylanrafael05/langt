using Langt.Codegen;

namespace Langt.AST;

public record UsingDeclaration(ASTToken Using, ASTNamespace Identifier) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Using, Identifier};

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {}
    public override void LowerSelf(CodeGenerator generator)
    {}

    public override void DefineTypes(ASTPassState state)
    {
        var ns = Identifier.Resolve(state);
        if(ns is null)
        {
            state.Error("Cannot have a 'using' declaration which uses a non-namespace", Range);
            return; 
        }

        state.CG.CurrentFile!.Scope.IncludedNamespaces.Add(ns);
    }
}