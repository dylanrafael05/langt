using Langt.Structure;

namespace Langt.AST;

public record OptionType(ASTType Left, ASTToken Pipe, ASTType Right) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Pipe, Right};

    public override ISymbol<LangtType> GetSymbol(Context ctx)
    {
        var l = Left.GetSymbol(ctx);
        var r = Right.GetSymbol(ctx);

        var options = new List<ISymbol<LangtType>> {l};

        if(r is OptionTypeSymbol ro)
        {
            options.AddRange(ro.OptionSymbols);
        }
        else 
        {
            options.Add(r);
        }

        return new OptionTypeSymbol
        {
            Range = Range,
            OptionSymbols = options
        };
    }
}