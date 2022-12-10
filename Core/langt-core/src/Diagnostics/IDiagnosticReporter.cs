namespace Langt;

public interface IDiagnosticReporter
{
    void Report(Diagnostic diagnostic);
    void HandleError();
    void HandleFatal();
}
