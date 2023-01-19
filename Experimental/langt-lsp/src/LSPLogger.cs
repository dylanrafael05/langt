using Langt.Codegen;

namespace Langt.LSP;

public class LSPLogger : ILogger
{
    IReadOnlySet<string> ILogger.DebugFlags { get; set; } 
        = Enumerable.Empty<string>().ToHashSet();

    public void Init()
    {}

    public void Log(MessageSeverity severity, string message)
    {
        Console.WriteLine($"{severity}: {message}");
    }

    public void Dispose()
    {}
}
