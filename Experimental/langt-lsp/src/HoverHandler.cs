using Langt.Codegen;
using Langt.Utility;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace Langt.LSP;

public class HoverHandler : HoverHandlerBase
{
    private readonly ILanguageServerFacade facade;
    private readonly ProjectManager proj;

    public HoverHandler(ILanguageServerFacade router, ProjectManager proj)
    {
        this.facade = router;
        this.proj = proj;
    }

    public override Task<Hover?> Handle(HoverParams request, CancellationToken cancellationToken)
    {
        var file = proj.GetFile(request.TextDocument);

        if(file is null) return Task.FromResult<Hover?>(null);

        var currentReference = proj.Project.References
            .Where(r => r.Range.Source == file.Source)
            .FirstOrDefault(r => r.Range.Contains(request.Position.ToLangt()));

        var contentStr = currentReference?.Item switch
        {
            LangtVariable lv when !lv.IsParameter
                => $"let {lv.Name} {lv.Type.GetFullName()}",
            LangtVariable lv
                => $"{lv.Name} {lv.Type.GetFullName()}",
            LangtType lt when lt is LangtStructureType 
                => $"struct {lt.GetFullName()}",
            LangtType lt when lt is LangtAliasType 
                => $"alias {lt.GetFullName()} = {lt.AliasBaseType.GetFullName()}",
            LangtFunction lf
                => $"let {lf.GetFullName()}({string.Join(", ", lf.ParameterNames.Select((n, i)=>n + " " + lf.Type.ParameterTypes[i].GetFullName()))}) {lf.Type.ReturnType.GetFullName()}",
            
            _ => null
        };

        if(contentStr is null) return Task.FromResult<Hover?>(null);

        return Task.FromResult<Hover?>(new Hover
        {
            Range = currentReference!.Range.ToVS(),
            Contents = new MarkedStringsOrMarkupContent(
                new MarkedString("langt", contentStr),
                currentReference.Item.Documentation
            )
        });
    }

    protected override HoverRegistrationOptions CreateRegistrationOptions(HoverCapability capability, ClientCapabilities clientCapabilities)
    {
        return new() 
        {
            DocumentSelector = LangtLanguageServer.DocumentSelector
        };
    }
}
