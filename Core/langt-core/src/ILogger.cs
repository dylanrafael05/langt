namespace Langt.Codegen;

public interface ILogger : IDisposable
{
    IReadOnlySet<string> DebugFlags {get; set;}

    void Init();

    void Log(MessageSeverity severity, string message);

    void Debug(string message, string flag) => Log(MessageSeverity.Debug(flag), message);
}

public static class Loggers
{
    public static void Note(this ILogger logger, string message)    => logger.Log(MessageSeverity.Note, message);
    public static void Warning(this ILogger logger, string message) => logger.Log(MessageSeverity.Warning, message);
    public static void Error(this ILogger logger, string message)   => logger.Log(MessageSeverity.Error, message);
    public static void Fatal(this ILogger logger, string message)   => logger.Log(MessageSeverity.Fatal, message);
}
