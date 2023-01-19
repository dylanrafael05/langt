using TT = Langt.Lexing.TokenType;

namespace Langt.Parsing;

public record struct ParseOperatorSpec(ParseOperatorType Type, params TT[] TokenTypes);
public record struct OperatorSpec(OperatorType Type, TT TokenType);