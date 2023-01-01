using Langt.Codegen;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Langt.LSP;
public class ProjectManager
{
    public LangtProject Project {get; private set;} = new(new LSPLogger(), "__lsp");
    public DiagnosticCollection Diagnostics => Project.Diagnostics;

    public bool ContainsFile(TextDocumentItem doc)
        => ContainsFile(doc.Uri);
    public bool ContainsFile(DocumentUri uri)
        => Project.Files.Any(p => p.Source.Name == uri.Path);

    public void AddFile(TextDocumentItem doc) 
        => AddFile(doc, doc.Text);
    public void AddFile(TextDocumentIdentifier doc, string content)
    {
        Project.AddFileContents(doc.Uri.Path, content);
        Project.BindSyntaxTrees();
    }

    public void RemoveFile(TextDocumentIdentifier doc, bool bind = true)
    {
        var newproj = new LangtProject(Project.Logger, Project.LLVMModuleName);

        foreach(var file in Project.Files.Where(f => f.Source.Name != doc.Uri.Path))
        {
            newproj.AddFileContents(file.Source.Name, file.Source.Content);
        }

        if(bind) newproj.BindSyntaxTrees();

        Project = newproj;
    }

    public void UpdateFile(TextDocumentItem doc)
        => UpdateFile(doc, doc.Text);
    public void UpdateFile(TextDocumentIdentifier doc, string content)
    {
        RemoveFile(doc, bind: false);
        AddFile(doc, content);
    }
}