using Langt.Structure.Visitors;
using Langt.Lexing;

namespace Langt.AST;

public record ASTToken(IReadOnlyList<Token> Prefix, Token Inner) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new();
    public override SourceRange Range => Inner.Range;

    public TokenType Type => Inner.Type;
    public TokenCategory Category => Inner.Category;
    public ReadOnlySpan<char> Content => Inner.Content;
    public string ContentStr => Inner.ContentStr;

    public string Documentation 
        => string.Join("\n", Prefix.Select(s => s.ContentStr[1..].TrimStart()));

    public override void Dump(VisitDumper visitor)
    {
        visitor.VisitNoDepth(Inner);
    }
}