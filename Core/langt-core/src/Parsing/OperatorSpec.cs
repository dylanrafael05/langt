using TT = Langt.Lexing.TokenType;

namespace Langt.Parsing;

public record struct OperatorSpec(OperatorType Type, params TT[] TokenTypes);
