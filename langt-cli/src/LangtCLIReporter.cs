namespace Langt;

public class LangtCLIReporter : Codegen.ILogger
{
    public void Log(DiagnosticSeverity severity, string message)
    {
        (Console.ForegroundColor, Console.BackgroundColor) = severity switch 
        {
            DiagnosticSeverity.Log     => (ConsoleColor.DarkGray, ConsoleColor.Black),
            DiagnosticSeverity.Note    => (ConsoleColor.DarkGray, ConsoleColor.Black),
            DiagnosticSeverity.Warning => (ConsoleColor.Yellow,   ConsoleColor.Black),
            DiagnosticSeverity.Error   => (ConsoleColor.Red,      ConsoleColor.Black),
            DiagnosticSeverity.Fatal   => (ConsoleColor.DarkRed,  ConsoleColor.Black),
            _                          => (ConsoleColor.Gray,     ConsoleColor.Black)
        };

        Console.Write(severity switch 
        {
            DiagnosticSeverity.Log     => "[Info]",
            DiagnosticSeverity.Warning => "[Warn]",
            DiagnosticSeverity.Error   => "[Error]",
            DiagnosticSeverity.Fatal   => "[Fatal]",
            _                          => "[Note]"
        });
        
        Console.Write(" ");
        Console.Write(message);

        Console.WriteLine();

        Console.ResetColor();
    }

    public void Abort() 
    {   
        Console.WriteLine("Aborting process . . . ");
        Console.WriteLine();

        Environment.Exit(1);
    }
}
