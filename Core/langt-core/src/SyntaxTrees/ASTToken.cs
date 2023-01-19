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

    private string? doc = null;
    public string Documentation => doc ??= GetDocumentation();

    public string GetDocumentation()
    {
        if(Prefix.Count == 0) return string.Empty;

        var s = new StringBuilder();
        var p = Prefix[^1];
        
        var anyContent = false;
        var includeNewline = false;

        void HandleContent(ReadOnlySpan<char> strin)
        {
            var str = strin.Trim();

            if(str.IsEmpty)
            {
                includeNewline = true;
                return;
            }

            if(includeNewline && anyContent)
            {
                s.AppendLine().AppendLine();
            }

            if(anyContent && !includeNewline)
            {
                s.Append(' ');
            }

            includeNewline = false;
            anyContent = true;

            s.Append(str);
        }

        if(p.Type is TokenType.Comment) 
        {
            HandleContent(p.Content[1..]);
        }
        else if(p.Type is TokenType.BlockComment) 
        {
            var content = p.Content[1..]
                .TrimStart('[')
                .TrimStart('(')
                .TrimEnd(']')
                .TrimEnd(')');

            foreach(var l in content.EnumerateLines())
            {
                HandleContent(l);
            }
        }

        return s.ToString();
    }

    public override void Dump(VisitDumper visitor)
    {
        visitor.VisitNoDepth(Inner);
    }
}