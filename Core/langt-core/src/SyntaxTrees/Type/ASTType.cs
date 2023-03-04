using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;
using Langt.Utility;


namespace Langt.AST;

/// <summary>
/// A special case of ASTNode which represents some type information
/// </summary>
public abstract record ASTType() : ASTNode, ISymbolProvider<LangtType>
{
    public abstract ISymbol<LangtType> GetSymbol(Context ctx);
}

public record FunctionPtrType(ASTToken Star, ASTToken Fn, ASTToken Open, SeparatedCollection<ASTType> Arguments, ASTToken? Ellipsis, ASTToken Close, ASTType ReturnType) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Star, Fn, Open, Arguments, Ellipsis, Close, ReturnType};

    public override ISymbol<LangtType> GetSymbol(Context ctx)
    {
        return new FunctionTypeSymbol
        {
            ReturnType = ReturnType.GetSymbol(ctx),
            ParameterTypes = Arguments.Values.Select(a => a.GetSymbol(ctx)).ToArray(),
            IsVararg = Ellipsis is not null
        };
    }
}