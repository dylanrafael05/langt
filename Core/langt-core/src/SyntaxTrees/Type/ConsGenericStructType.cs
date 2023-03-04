using Langt.Structure;

namespace Langt.AST;

public record ConsGenericType(ASTType Type, ASTToken GenTok, ASTToken Open, SeparatedCollection<ASTType> Arguments, ASTToken Close) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Type, GenTok, Open, Arguments, Close};

    public override ISymbol<LangtType> GetSymbol(Context ctx)
    {
        var builder = ResultBuilder.Empty();

        var baseTy = Type.GetSymbol(ctx);
        var args = Arguments.Values.Select(a => a.GetSymbol(ctx));

        return new GenericTypeSymbol {Base = baseTy, Arguments = args.ToArray()};
    }
}
