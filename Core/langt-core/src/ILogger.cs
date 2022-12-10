namespace Langt.Codegen;

public interface ILogger
{
    void Log(DiagnosticSeverity severity, string message);

    void Note(string message) => Log(DiagnosticSeverity.Note, message);
    void Warning(string message) => Log(DiagnosticSeverity.Warning, message);
    void Error(string message) => Log(DiagnosticSeverity.Error, message);
    void Fatal(string message) => Log(DiagnosticSeverity.Fatal, message);
}
