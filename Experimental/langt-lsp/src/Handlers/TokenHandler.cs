using Langt.AST;
using Langt.Structure;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Window;

namespace Langt.LSP;

public class TokenHandler : SemanticTokensHandlerBase
{
    private readonly IMLogger logger;
    private readonly ILanguageServerFacade facade;
    private readonly ProjectManager proj;

    public TokenHandler(ILogger<TokenHandler> logger, ILanguageServerFacade router, ProjectManager proj)
    {
        this.logger = logger;
        this.facade = router;
        this.proj = proj;
    }

    protected override SemanticTokensRegistrationOptions CreateRegistrationOptions(SemanticTokensCapability capability, ClientCapabilities clientCapabilities)
    {
        var s = new SemanticTokensRegistrationOptions()
        {
            DocumentSelector = LangtLanguageServer.DocumentSelector,
            Legend = new SemanticTokensLegend()
            {
                TokenTypes     = capability.TokenTypes,
                TokenModifiers = new(capability.TokenModifiers.Concat(SemanticToken.NewModifiers))
            },
            Full = new SemanticTokensCapabilityRequestFull
            {
                Delta = true
            },
            Range = true
        };

        return s;
    }

    protected override Task<SemanticTokensDocument> GetSemanticTokensDocument(ITextDocumentIdentifierParams @params, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SemanticTokensDocument(RegistrationOptions.Legend!));
    }

    protected override async Task Tokenize(SemanticTokensBuilder builder, ITextDocumentIdentifierParams identifier, CancellationToken cancellationToken)
    {
        facade.Window.LogMessage(new() {Message="Hello from the tokenizer!"});

        var f = proj.GetFile(identifier.TextDocument)!;
        await Task.Yield();

        foreach(var reference in proj.Project.References.Where(r => r.Range.Source == f.Source))
        {
            var spec = reference.Item switch 
            {
                LangtFunction or LangtFunctionGroup => SemanticToken.From(STT.Function),
                LangtVariable v when v.IsParameter  => SemanticToken.From(STT.Parameter),
                LangtVariable v when !v.IsParameter => SemanticToken.From(STT.Variable),
                LangtType t when t.IsStructure      => SemanticToken.From(STT.Struct),
                LangtType t when t.IsAlias          => SemanticToken.From(STT.Type, SemanticToken.Alias),
                LangtType t when t.IsBuiltin        => SemanticToken.From(STT.Type, SemanticToken.Builtin),
                LangtNamespace                      => SemanticToken.From(STT.Namespace),
                _                                   => SemanticToken.None
            };

            builder.Push(reference.Range.ToVS(), spec.Type, spec.Modifiers);
        }
    }

    // necessary by OmniSharp example (?)
    public override async Task<SemanticTokens?> Handle(
        SemanticTokensParams request, CancellationToken cancellationToken
    )
    {
        var result = await base.Handle(request, cancellationToken).ConfigureAwait(false);
        return result;
    }

    public override async Task<SemanticTokens?> Handle(
        SemanticTokensRangeParams request, CancellationToken cancellationToken
    )
    {
        var result = await base.Handle(request, cancellationToken).ConfigureAwait(false);
        return result;
    }

    public override async Task<SemanticTokensFullOrDelta?> Handle(
        SemanticTokensDeltaParams request,
        CancellationToken cancellationToken
    )
    {
        var result = await base.Handle(request, cancellationToken).ConfigureAwait(false);
        return result;
    }
}
