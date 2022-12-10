namespace Langt.Structure.Visitors;

// TODO: create variant with visit functionality in visitor class instead of in element class
public abstract class Visitor<TSelf> where TSelf : Visitor<TSelf>
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
    where TVisitor : Visitor<TVisitor>
{

    void OnVisit(TVisitor visitor);
}