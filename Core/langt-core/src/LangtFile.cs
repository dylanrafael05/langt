using Langt.AST;
using Langt.Lexing;
using Langt.Parsing;

namespace Langt.Codegen;

public class LangtFile
{
    public Source Source {get; private set;}
    public ASTNode AST {get; private set;}
    public BoundASTNode? BoundAST {get; set;}
    public LangtFileScope Scope {get; private set;}

    public LangtFile(LangtProject project, Source source)
    {
        Source = source;
        Scope = new(project.GlobalScope);
        
        project.Logger.Note("Lexing " + source.Name + " . . . ");
        var lex = Lexer.Lex(source, project);

        project.Logger.Note("Parsing " + source.Name + " . . . ");
        AST = Parser.Parse(lex, project);
    }
}
