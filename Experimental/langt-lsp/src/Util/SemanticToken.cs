namespace Langt.LSP;

public record SemanticToken(STT? Type, params STM[] Modifiers)
{
    public static SemanticToken From(STT? Type, params STM[] Modifiers)
        => new(Type, Modifiers);
    public static SemanticToken None => new((STT?)null);
    
    public static STM Builtin => new("builtin");
    public static STM Alias   => new("alias");

    public static readonly STM[] NewModifiers = 
    {
        Builtin,
        Alias
    };
}
