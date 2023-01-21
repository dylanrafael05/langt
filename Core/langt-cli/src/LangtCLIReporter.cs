using System.Reflection;
using Langt.Structure;

namespace Langt;

public sealed class CLILogger : ILogger
{
    // TODO; implement more complex debug flags
    // 'flag:N' for verbosities 0..N provided to Debug diagnostic, '_:N' all for given verbosities, '_:_' all
    public IReadOnlySet<string> DebugFlags {get; set;} = new HashSet<string>();

    private readonly IDictionary<string, StringBuilder> flagFileBuilders 
        = new Dictionary<string, StringBuilder>();

    public static string GetLogFile(string flag)
        => Path.Combine(Environment.CurrentDirectory!, $"__log.{flag}.txt");

    public void Init() 
    {
        #if DEBUG
        flagFileBuilders.Clear();

        foreach(var flag in DebugFlags)
        {
            flagFileBuilders[flag] = new();
        }
        #endif
    }
    
    public void Dispose()
    {
        //#if DEBUG
        foreach(var (n, b) in flagFileBuilders)
        {
            File.WriteAllText(GetLogFile(n), b.ToString());
        }
        //#endif
    }

    public void Log(MessageSeverity severity, string message)
    {
        #if DEBUG
        if(!severity.ShouldDisplay(DebugFlags)) return;
        #endif

        var (fg, bg) = severity.SeverityType switch 
        {
            MessageSeverityType.Debug   => (ConsoleColor.White,    ConsoleColor.Black),
            MessageSeverityType.Log     => (ConsoleColor.DarkGray, ConsoleColor.Black),
            MessageSeverityType.Note    => (ConsoleColor.DarkGray, ConsoleColor.Black),
            MessageSeverityType.Warning => (ConsoleColor.Yellow,   ConsoleColor.Black),
            MessageSeverityType.Error   => (ConsoleColor.Red,      ConsoleColor.Black),
            MessageSeverityType.Fatal   => (ConsoleColor.DarkRed,  ConsoleColor.Black),
            _                           => (ConsoleColor.Gray,     ConsoleColor.Black)
        };

        var resultantMessage = new StringBuilder();

        resultantMessage.Append(severity.SeverityType switch 
        {
            MessageSeverityType.Debug   => "",
            MessageSeverityType.Log     => "[Info] ",
            MessageSeverityType.Warning => "[Warn] ",
            MessageSeverityType.Error   => "[Error] ",
            MessageSeverityType.Fatal   => "[Fatal] ",
            _                           => "[Note] "
        })
            .Append(message)
            .AppendLine();

        #if DEBUG
        if(severity.IsDebug)
        {
            flagFileBuilders[severity.Flag!].Append(resultantMessage);
        }
        else
        #endif
        {
            var rm = resultantMessage.ToString();

            (Console.ForegroundColor, Console.BackgroundColor) = (fg, bg);

            Console.Write(rm);

            #if DEBUG
            foreach(var fb in flagFileBuilders.Values)
            {
                fb!.Append(rm);
            }
            #endif

            Console.ResetColor();
        }
    }

    public static void Abort() 
    {   
        Console.WriteLine("Aborting process . . . ");
        Console.WriteLine();

        Environment.Exit(1);
    }
}
