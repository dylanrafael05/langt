using Langt.Structure.Visitors;
using Langt.Lexing;

namespace Langt.AST;

public record ASTToken(Token Inner) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new();
    public override SourceRange Range => Inner.Range;

    public TokenType Type => Inner.Type;
    public TokenCategory Category => Inner.Category;
    public ReadOnlySpan<char> Content => Inner.Content;
    public string ContentStr => Inner.ContentStr;

    public override void Dump(VisitDumper visitor)
    {
        visitor.VisitNoDepth(Inner);
    }
}