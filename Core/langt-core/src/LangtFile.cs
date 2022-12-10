using Langt.AST;
using Langt.Lexing;
using Langt.Parsing;

namespace Langt.Codegen;

public class LangtFile
{
    public Source Source {get; private set;}
    public CompilationUnit CompilationUnit {get; private set;}
    public LangtFileScope Scope {get; private set;}

    public LangtFile(LangtProject project, Source source)
    {
        Source = source;
        Scope = new(project.GlobalScope);
        
        project.Logger.Note("Lexing " + source.Name + " . . . ");
        var lex = Lexer.Lex(source, project);

        project.Logger.Note("Parsing " + source.Name + " . . . ");
        CompilationUnit = Parser.Parse(lex, project);
    }
}
