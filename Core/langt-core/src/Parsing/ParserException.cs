namespace Langt.Parsing;

[Serializable]
public class ParserException : Exception
{
    public override string Message {get;}
    public MessageSeverityType Severity {get; init;}

    private ParserException(string message, MessageSeverityType severity) 
    {
        Message = message;
        Severity = severity;
    }

    public static ParserException Create(string message, MessageSeverityType severity = MessageSeverityType.Fatal)
        => new(message, severity);
    public static ParserException Error(string message)
        => new(message, MessageSeverityType.Error);
    public static ParserException Fatal(string message)
        => new(message, MessageSeverityType.Fatal);
}
