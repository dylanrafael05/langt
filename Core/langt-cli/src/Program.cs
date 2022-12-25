using System.Runtime.InteropServices;
using Langt.Lexing;
using Langt.AST;
using Langt.Utility;
using Langt.Codegen;
using Langt.Optimization;
using Langt.Parsing;
using Langt.Structure.Visitors;
using Langt;
using System.CommandLine;

namespace Langt.CLI;

public static class Program
{
    public static bool Noisy {get; private set;}
    public static bool Debug {get; private set;}

    public static async Task Main(string[] args)
    {
#if DEBUG
        if(args.Length == 0)
            args = Console.ReadLine()!.Split(" ");
#endif

        var root = new RootCommand("The compiler for the *Langt* language, a language created by [Dylan Rafael] in his spare time!");

            var noisy = new Option<bool>("--noisy", "Whether or not to log information while building");

                noisy.AddAlias("-n");
            
            root.AddGlobalOption(noisy);

            var debugFlags = new Option<string[]>("--debug", "The flags to set debug for")
            {
                IsHidden = true,
                Arity = ArgumentArity.ZeroOrMore,
                AllowMultipleArgumentsPerToken = true
            };
            debugFlags.AddAlias("-d");
            root.AddGlobalOption(debugFlags);

            var run = new Command("run", """Runs the provided Langt file or directory through by means of its "main" function""");

                var runinput = new Argument<string>("input", "A path to the input file");

                    runinput.AddValidator(r => 
                    {
                        var filename = r.GetValueOrDefault<string>();

                        if(!File.Exists(filename))
                        {
                            if(!Directory.Exists(filename))
                            {
                                r.ErrorMessage = "Could not find " + filename + "" + Environment.NewLine + "Please enter a valid filename";
                            }
                        }
                        else if(Path.GetExtension(filename) is not (".txt" or ".lgt"))
                        {
                            r.ErrorMessage = "Input file must be either a .lgt file or a .txt file";
                        }
                    });

                run.Add(runinput);

            run.SetHandler(OnRun, runinput, noisy, debugFlags);
            root.Add(run);

        root.TreatUnmatchedTokensAsErrors = true;
        await root.InvokeAsync(args);
    }

    public static void SetFlags(bool noisy)
    {
        Noisy = noisy;
    }

    public static void OnRun(string input, bool noisy, string[] debugFlags)
    {
        SetFlags(noisy);

        Console.WriteLine();

        using ILogger logger = new CLILogger()
        {
            DebugFlags = debugFlags.ToHashSet()
        };

        var proj = new LangtProject(logger, input);

        proj.LoadFromFileOrDirectory(input);
        proj.Build();

        proj.LogAllDiagnostics();

        if(proj.Diagnostics.AnyErrors)
        {
            CLILogger.Abort();
        }

        logger.Note("Running function main in " + input + " . . .");

        try
        {
            LLVMUtil.CallMain(proj.Module!.Value, logger);
        }
        catch(Exception e) 
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("An error occured: " + e.Message);
            Console.WriteLine("Press enter to continue, press space to see error stack.");

            if(Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                Console.WriteLine();
                Console.Write(e.ToString());
            }

            Console.ResetColor();
            Environment.Exit(1);
        }
        
        Console.WriteLine();
        logger.Note("Finished running!");
        Console.WriteLine();
    }
}