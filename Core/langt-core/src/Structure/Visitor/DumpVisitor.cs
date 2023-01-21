using System.Text;

namespace Langt.Structure.Visitors;

public class VisitDumper : DepVisitor<VisitDumper>
{
    private Stack<bool> skipDepthStack = new();
    public bool SkipDepth {get; set;} = false;

    public void VisitNoDepth(IElement<VisitDumper> element) 
    {
        SkipDepth = true;
        Visit(element);
    }

    public int Depth {get; private set;} = -1;

    private StringBuilder builder = new();

    public override void OnEnter()
    {
        if(!SkipDepth)
        {
            Depth++;
        }
        
        skipDepthStack.Push(SkipDepth);
        SkipDepth = false;
    }

    public override void OnLeave()
    {
        SkipDepth = skipDepthStack.Pop();

        if(!SkipDepth)
        {
            Depth--;
        }
    }

    public void PutString(string text)
    {
        builder
            .Append(string.Concat(Enumerable.Repeat("|  ", Math.Max(0, Depth))))
            .Append(text)
            .AppendLine();
    }

    public string Content => builder.ToString();

    public static string Dump(IElement<VisitDumper> element)
    {
        var v = new VisitDumper();
        v.Visit(element);
        return v.Content;
    }
}