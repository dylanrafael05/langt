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

    public void RebuildProject(TextDocumentIdentifier? docToExclude = null)
    {
        var newproj = new LangtProject(Project.Logger, Project.LLVMModuleName);

        foreach(var file in Project.Files)
        {
            if(docToExclude is not null && file.Source.Name == docToExclude.Uri.Path)
                continue;

            newproj.AddFileContents(file.Source.Name, file.Source.Content);
        }

        Project = newproj;
    }

    public void AddFile(TextDocumentItem doc) 
        => AddFile(doc, doc.Text);
    public void AddFile(TextDocumentIdentifier doc, string content)
    {
        RebuildProject();
        Project.AddFileContents(doc.Uri.Path, content);
        Project.BindSyntaxTrees();
    }

    public void RemoveFile(TextDocumentIdentifier doc)
    {
        RebuildProject(doc);
    }

    public void UpdateFile(TextDocumentItem doc)
        => UpdateFile(doc, doc.Text);
    public void UpdateFile(TextDocumentIdentifier doc, string content)
    {
        RemoveFile(doc);
        AddFile(doc, content);
    }

    public LangtFile? GetFile(TextDocumentIdentifier doc) 
        => Project.Files.FirstOrDefault(f => f.Source.Name == doc.Uri.Path);
}