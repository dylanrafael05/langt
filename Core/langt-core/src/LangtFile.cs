using Langt.AST;
using Langt.Lexing;
using Langt.Parsing;
using Langt.Structure;
using Langt.Structure.Resolutions;

namespace Langt;

public class LangtFile
{
    public Source Source {get; private set;}
    public ASTNode AST {get; private set;}
    public BoundASTNode? BoundAST {get; set;}
    public IScope Scope {get; private set;}

    public LangtFile(LangtProject project, Source source)
    {
        Source = source;
        Scope = new LangtFileScope(project.GlobalScope);
        
        project.Logger.Note("Lexing " + source.Name + " . . . ");
        var lex = Lexer.Lex(source, project);

        project.Logger.Note("Parsing " + source.Name + " . . . ");
        AST = Parser.Parse(lex, project);
    }

    public void RebaseScope(IScope newBase) 
    {
        Scope = new LangtFileScope(newBase);
    }
    public void IncludeNamespace(LangtNamespace ns) 
    {
        var s = (LangtFileScope)Scope!;
        s.IncludedNamespaces.Add(ns);
    }
}
