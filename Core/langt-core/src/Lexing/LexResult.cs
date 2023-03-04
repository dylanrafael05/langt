using System.Collections;
using Langt.Structure.Collections;

namespace Langt.Lexing;

public class LexResult : IReadOnlyList<Token>
{
    private readonly List<Token> tokens;
    public IReadOnlyList<Token> Tokens => tokens;

    public Token this[int index] => tokens[index];
    public int Count => tokens.Count;

    public IEnumerator<Token> GetEnumerator() 
        => ((IEnumerable<Token>)tokens).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() 
        => ((IEnumerable)tokens).GetEnumerator();

    public LexResult(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public string Dump()
    {
        return string.Join(Environment.NewLine, tokens);
    }
}