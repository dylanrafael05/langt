using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Langt.LSP;

public class GotoDefinitionHandler : DefinitionHandlerBase
{    
    private readonly ProjectManager proj;

    public GotoDefinitionHandler(ProjectManager proj)
    {
        this.proj = proj;
    }

    public override Task<LocationOrLocationLinks> Handle(DefinitionParams request, CancellationToken cancellationToken)
    {
        var file = proj.GetFile(request.TextDocument);

        if(file is null) return Task.FromResult(new LocationOrLocationLinks());

        var currentReference = proj.GetReferenceAt(file, request.Position);

        if(currentReference is not null && currentReference.Item.DefinitionRange is LangtRange sr)
        {
            return Task.FromResult
            (
                new LocationOrLocationLinks
                (
                    new LocationLink() 
                    {
                        TargetSelectionRange = sr.ToVS(),
                        TargetRange = sr.ToVS(),

                        TargetUri = proj.SourceToURI[sr.Source],

                        OriginSelectionRange = currentReference.Range.ToVS()
                    }
                )
            );
        }

        return Task.FromResult(new LocationOrLocationLinks());
    }

    protected override DefinitionRegistrationOptions CreateRegistrationOptions(DefinitionCapability capability, ClientCapabilities clientCapabilities)
    {
        return new DefinitionRegistrationOptions
        {
            DocumentSelector = LangtLanguageServer.DocumentSelector
        };
    }
}
