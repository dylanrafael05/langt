using Langt.Codegen;

namespace Langt.AST;

public record DefineAlias(ASTToken Alias, ASTToken Name, ASTToken Eq, ASTType Type) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Alias, Name, Eq, Type};

    public LangtAliasType? AliasType {get; private set;}

    public override void DefineTypes(ASTPassState state)
    {
        var t = new LangtAliasType(Name.ContentStr);
        state.CG.ResolutionScope.DefineType(t, Range, state);

        AliasType = t;
    }

    public override void ImplementTypes(ASTPassState state)
    {
        AliasType!.SetBase(Type.Resolve(state));
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {}

    public override void LowerSelf(CodeGenerator generator)
    {}
}
