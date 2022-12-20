using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

/// <summary>
/// A special case of ASTNode which represents some type information
/// </summary>
public abstract record ASTType() : ASTNode //TODO: implement distinction between type definition and implementation
{
    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {}
    public override void LowerSelf(CodeGenerator lowerer)
    {}
    
    public abstract LangtType? Resolve(ASTPassState state);
}

public record FunctionPtrType(ASTToken Star, ASTToken Open, SeparatedCollection<ASTType> Arguments, ASTToken? Ellipsis, ASTToken Close, ASTType ReturnType) : ASTType
{
    public override ASTChildContainer ChildContainer => new() {Star, Open, Arguments, Ellipsis, Close, ReturnType};

    public override LangtType? Resolve(ASTPassState state)
    {
        var fType = new LangtFunctionType
        (
            ReturnType.Resolve(state) ?? LangtType.Error, 
            Ellipsis is not null, 
            Arguments.Values.Select(t => t.Resolve(state) ?? LangtType.Error).ToArray()
        );

        return new LangtPointerType(fType);
    }
}