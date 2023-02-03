using Spectre.Console;

namespace Langt.Utility;

public class TreeBuilder
{
    private string RawContent {get; set;}
    public Markup Content {get; private set;}
    public IEnumerable<TreeBuilder> Children {get; private set;}

    private TreeBuilder(string rawContent, IEnumerable<TreeBuilder> children) 
    {
        this.RawContent = rawContent;
        this.Content = new(rawContent);
        this.Children = children;
    }

    public Tree Build(Style? s = null, TreeGuide? g = null)
    {
        var t = new Tree(Content)
        {
            Style = s ?? Style.Plain,
            Guide = g ?? TreeGuide.Ascii
        };
        t.AddNodes(Children.Select(x => x.Build(s, g)));

        return t;
    }

    public static TreeBuilder From(string str, params TreeBuilder[] children) 
        => new(new(str), children);

    public void ModifyContent(Func<string, string> mod)
    {
        RawContent = mod(RawContent);
        Content = new(RawContent);
    }

    public void AddNode(params TreeBuilder[] t) 
    {
        Children = Children.Concat(t);
    }
}

public interface ITreeRenderable
{
    TreeBuilder ToStringTree();
}
