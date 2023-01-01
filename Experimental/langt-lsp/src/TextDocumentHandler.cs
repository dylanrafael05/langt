using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Window;

namespace Langt.LSP;
using VSDiagnostic = OmniSharp.Extensions.LanguageServer.Protocol.Models.Diagnostic;

public class TextDocumentHandler : TextDocumentSyncHandlerBase
{
    private readonly ILanguageServerFacade facade;
    private readonly ProjectManager proj;

    public TextDocumentHandler(ILanguageServerFacade router, ProjectManager proj)
    {
        this.facade = router;
        this.proj = proj;
    }

    public TextDocumentChangeRegistrationOptions GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
    {
        throw new NotImplementedException();
    }

    public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri) 
        => new(uri, LangtLanguageServer.LanguageID);

    public override Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
        if(proj.ContainsFile(request.TextDocument))
        {
            proj.UpdateFile(request.TextDocument);
        }
        else 
        {
            proj.AddFile(request.TextDocument);
        }

        PublishDiagnostics(request.TextDocument);
        
        return Unit.Task;
    }

    public override Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
        var ch = request.ContentChanges.FirstOrDefault()?.Text;

        if(ch is not null)
        {
            proj.UpdateFile(request.TextDocument, ch);
        }

        PublishDiagnostics(request.TextDocument);

        return Unit.Task;
    }

    public override Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    public override Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
    {
        proj.RemoveFile(request.TextDocument);
        return Unit.Task;
    }

    public void PublishDiagnostics(OptionalVersionedTextDocumentIdentifier doc) 
        => PublishDiagnostics(doc.Uri, doc.Version);
    public void PublishDiagnostics(TextDocumentItem doc) 
        => PublishDiagnostics(doc.Uri, doc.Version);
    public void PublishDiagnostics(DocumentUri uri, int? version)
    {
        var rdiag = new List<VSDiagnostic>();
        foreach(var d in proj.Project.Diagnostics)
        {
            if(d.Range.Source.Name != uri.Path) continue;
            rdiag.Add(d.ToVS());
        }

        facade.TextDocument.PublishDiagnostics(new() 
        {
            Diagnostics = new(rdiag),
            Uri = uri,
            Version = version
        });
    }

    protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
        => new()
        {
            Change = LangtLanguageServer.SyncKind,
            DocumentSelector = LangtLanguageServer.DocumentSelector
        };
}