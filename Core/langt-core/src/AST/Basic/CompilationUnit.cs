using Langt.Codegen;

namespace Langt.AST;

public record CompilationUnit(StatementGroup Group) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Group};

    public override bool BlockLike => true;

    public override void Initialize(CodeGenerator generator)
    {
        Group.Initialize(generator);
    }
    public override void DefineTypes(CodeGenerator generator)
    {
        Group.DefineTypes(generator);
    }
    public override void ImplementTypes(CodeGenerator generator)
    {
        Group.ImplementTypes(generator);
    }
    public override void DefineFunctions(CodeGenerator generator)
    {
        Group.DefineFunctions(generator);
    }
    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Group.TypeCheck(generator);
    }
    public override void LowerSelf(CodeGenerator lowerer)
    {
        Group.Lower(lowerer);
    }
}
