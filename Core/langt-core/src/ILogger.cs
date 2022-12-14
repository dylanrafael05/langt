namespace Langt.Codegen;

public interface ILogger : IDisposable
{
    IReadOnlySet<string> DebugFlags {get; set;}

    void Init();

    void Log(MessageSeverity severity, string message);

    void Debug(string message, string flag) => Log(MessageSeverity.Debug(flag), message);

    void Note(string message)    => Log(MessageSeverity.Note, message);
    void Warning(string message) => Log(MessageSeverity.Warning, message);
    void Error(string message)   => Log(MessageSeverity.Error, message);
    void Fatal(string message)   => Log(MessageSeverity.Fatal, message);
}
