using Langt.Codegen;

namespace Langt.AST;

public record NamespaceDeclaration(ASTToken Namespace, ASTNamespace Identifier) : ASTNode
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Namespace, Identifier};

    public LangtNamespace? LNamespace {get; set;}

    public override void DefineTypes(ASTPassState state)
    {
        if(LNamespace is null) return;
        state.CG.SetCurrentNamespace(LNamespace!);
    }
    public override void ImplementTypes(ASTPassState state)
    {
        if(LNamespace is null) return;
        state.CG.SetCurrentNamespace(LNamespace!);
    }
    public override void DefineFunctions(ASTPassState state)
    {
        if(LNamespace is null) return;
        state.CG.SetCurrentNamespace(LNamespace!);
    }
    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        if(LNamespace is null) return;
        state.CG.SetCurrentNamespace(LNamespace!);
    }
    public override void LowerSelf(CodeGenerator generator)
    {}

    public override void Initialize(ASTPassState state)
    {
        LNamespace = Identifier.Resolve(state, true);
        if(LNamespace is null) throw new Exception();
    }
}
