using Langt.Codegen;

namespace Langt.AST;

public record NamespaceDeclaration(ASTToken Namespace, ASTNamespace Identifier) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Namespace, Identifier};

    public LangtNamespace? LNamespace {get; set;}

    public override void DefineTypes(CodeGenerator generator)
    {
        if(LNamespace is null) return;
        generator.SetCurrentNamespace(LNamespace!);
    }
    public override void ImplementTypes(CodeGenerator generator)
    {
        if(LNamespace is null) return;
        generator.SetCurrentNamespace(LNamespace!);
    }
    public override void DefineFunctions(CodeGenerator generator)
    {
        if(LNamespace is null) return;
        generator.SetCurrentNamespace(LNamespace!);
    }
    public override void TypeCheckRaw(CodeGenerator generator)
    {
        if(LNamespace is null) return;
        generator.SetCurrentNamespace(LNamespace!);
    }
    public override void LowerSelf(CodeGenerator generator)
    {}

    public override void Initialize(CodeGenerator generator)
    {
        LNamespace = Identifier.Resolve(generator, true);
        if(LNamespace is null) throw new Exception();
    }
}
