using System.Collections;

namespace Langt.Lexing;

public class LexResult : IReadOnlyList<Token>
{
    private readonly List<Token> tokens;

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
    public LexResult(IEnumerable<Token> tokens) : this(new(tokens))
    {}

    public string Dump()
    {
        return string.Join(Environment.NewLine, tokens);
    }
}