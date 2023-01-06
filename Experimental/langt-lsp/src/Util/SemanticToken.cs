namespace Langt.LSP;

using STT = OmniSharp.Extensions.LanguageServer.Protocol.Models.SemanticTokenType;
using STM = OmniSharp.Extensions.LanguageServer.Protocol.Models.SemanticTokenModifier;

public record SemanticToken(STT? Type, params STM[] Modifiers)
{
    public static SemanticToken From(STT? Type, params STM[] Modifiers)
        => new(Type, Modifiers);
    public static SemanticToken None => new((STT?)null);
}
