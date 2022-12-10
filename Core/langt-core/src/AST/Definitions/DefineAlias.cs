using Langt.Codegen;

namespace Langt.AST;

public record DefineAlias(ASTToken Alias, ASTToken Name, ASTToken Eq, ASTType Type) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Alias, Name, Eq, Type};

    public LangtAliasType? AliasType {get; private set;}

    public override void DefineTypes(CodeGenerator generator)
    {
        var t = new LangtAliasType(Name.ContentStr);
        generator.ResolutionScope.DefineType(t, Range, generator.Diagnostics);

        AliasType = t;
    }

    public override void ImplementTypes(CodeGenerator generator)
    {
        AliasType!.SetBase(Type.Resolve(generator));
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {}

    public override void LowerSelf(CodeGenerator generator)
    {}
}
