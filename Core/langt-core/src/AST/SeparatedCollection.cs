using Langt.Lexing;

namespace Langt.AST;

public record SeparatedCollection<T> : ASTNode where T : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {items};

    private readonly List<ASTNode> items;

    public SeparatedCollection(List<ASTNode> items) 
    {
        this.items = items;
    }

    public IEnumerable<T> Values 
    {
        get 
        {
            for(int i = 0; i < items.Count; i += 2)
            {
                yield return (T)items[i];
            }
        }
    }
    public IEnumerable<ASTToken> Separators 
    {
        get 
        {
            for(int i = 1; i < items.Count; i += 2)
            {
                yield return (ASTToken)items[i];
            }
        }
    }
    
    public IEnumerable<ASTNode> All 
        => items;
}