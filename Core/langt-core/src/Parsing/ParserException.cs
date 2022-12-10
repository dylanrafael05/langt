namespace Langt.Parsing;

[Serializable]
public class ParserException : Exception
{
    public override string Message {get;}
    public DiagnosticSeverity Severity {get; init;}

    private ParserException(string message, DiagnosticSeverity severity) 
    {
        Message = message;
        Severity = severity;
    }

    public static ParserException Create(string message, DiagnosticSeverity severity = DiagnosticSeverity.Fatal)
        => new(message, severity);
    public static ParserException Error(string message)
        => new(message, DiagnosticSeverity.Error);
    public static ParserException Fatal(string message)
        => new(message, DiagnosticSeverity.Fatal);
}
