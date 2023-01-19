using Langt.Lexing;

namespace Langt.AST;

public record SeparatedCollection<T>(List<ASTNode> All) : ASTNode where T : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {All};

    public IEnumerable<T> Values 
    {
        get 
        {
            for(int i = 0; i < All.Count; i += 2)
            {
                yield return (T)All[i];
            }
        }
    }
    public IEnumerable<ASTToken> Separators 
    {
        get 
        {
            for(int i = 1; i < All.Count; i += 2)
            {
                yield return (ASTToken)All[i];
            }
        }
    }
}