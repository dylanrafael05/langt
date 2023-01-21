namespace Langt.Structure.Visitors;

// TODO: create variant with visit functionality in visitor class instead of in element class
[Obsolete("Use new Visitor class instead, and replace DumpVisitor with PrettyPrintStructure")]
public abstract class DepVisitor<TSelf> where TSelf : DepVisitor<TSelf>
{
    private readonly Stack<IElement<TSelf>> stack = new();
    public IElement<TSelf> Current => stack.Peek();

    public virtual void OnEnter() {}
    public virtual void OnLeave() {}

    public void Visit(IElement<TSelf> t)
    {
        stack.Push(t);

        OnEnter();
        
        Current.OnVisit((TSelf)this);

        OnLeave();

        stack.Pop();
    }
}

public interface IElement<TVisitor>
    where TVisitor : DepVisitor<TVisitor>
{

    void OnVisit(TVisitor visitor);
}