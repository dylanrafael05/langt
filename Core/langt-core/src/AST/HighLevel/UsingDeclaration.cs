using Langt.Codegen;

namespace Langt.AST;

public record UsingDeclaration(ASTToken Using, ASTNamespace Identifier) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Using, Identifier};

    public override void TypeCheckRaw(CodeGenerator generator)
    {}
    public override void LowerSelf(CodeGenerator generator)
    {}

    public override void DefineTypes(CodeGenerator generator)
    {
        var ns = Identifier.Resolve(generator);
        if(ns is null)
        {
            generator.Diagnostics.Error("Cannot have a 'using' declaration which uses a non-namespace", Range);
            return; 
        }

        generator.CurrentFile!.Scope.IncludedNamespaces.Add(ns);
    }
}