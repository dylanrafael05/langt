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
    public LangtProject Project {get; init;}

    public LangtFile(LangtProject project, Source source)
    {
        Source = source;
        
        var fs = new FileScope(project);
        fs.SetDefScope(project.GlobalScope);

        Scope = fs;
        
        project.Logger.Note("Lexing " + source.Name + " . . . ");
        var lex = Lexer.Lex(source, project);

        project.Logger.Note("Parsing " + source.Name + " . . . ");
        AST = Parser.Parse(lex, project);

        Project = project;
    }

    public void RebaseScope(IScope newBase) 
    {
        ((FileScope)Scope).SetDefScope(newBase);
    }
    public void IncludeNamespace(Namespace ns) 
    {
        var s = (FileScope)Scope!;
        s.AddNamespace(ns);
    }
}
