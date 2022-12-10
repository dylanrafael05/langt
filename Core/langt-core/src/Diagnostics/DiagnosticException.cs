namespace Langt;

[Serializable]
public class DiagnosticException : Exception
{
    public Diagnostic Diagnostic {get; init;}
    public DiagnosticException(Diagnostic diagnostic) {Diagnostic = diagnostic;}
}
