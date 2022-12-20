using Langt.Codegen;

namespace Langt.AST;

public record CompilationUnit(StatementGroup Group) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Group};

    public override bool BlockLike => true;

    public override void Initialize(ASTPassState state)
    {
        TryPass(Group.Initialize, state);
    }
    public override void DefineTypes(ASTPassState state)
    {
        TryPass(Group.DefineTypes, state);
    }
    public override void ImplementTypes(ASTPassState state)
    {
        TryPass(Group.ImplementTypes, state);
    }
    public override void DefineFunctions(ASTPassState state)
    {
        TryPass(Group.DefineFunctions, state);
    }
    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        Group.TryTypeCheck(state);
    }
    public override void LowerSelf(CodeGenerator lowerer)
    {
        Group.Lower(lowerer);
    }
}
