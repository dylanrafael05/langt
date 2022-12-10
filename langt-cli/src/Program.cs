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
            
            var debug = new Option<bool>("--debug", "Whether or not to log debug information while building");

                debug.AddAlias("-d");
            
            root.AddGlobalOption(debug);

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

            run.SetHandler(OnRun, runinput, noisy, debug);
            root.Add(run);

        root.TreatUnmatchedTokensAsErrors = true;
        await root.InvokeAsync(args);
    }

    public static void SetFlags(bool noisy, bool debug)
    {
        Noisy = noisy;
        Debug = debug;
    }

    public static void OnRun(string input, bool noisy, bool debug)
    {
        SetFlags(noisy, debug);

        Console.WriteLine();

        var cliLogger = new LangtCLIReporter();
        var logger = cliLogger as Codegen.ILogger;
        var proj = new Codegen.LangtProject(cliLogger, input);

        proj.LoadFromFileOrDirectory(input);
        proj.Build();

        foreach(var d in proj.Diagnostics)
        {
            logger.Log(d.Severity, d.Message + " at " + d.Range);
        }

        if(proj.Diagnostics.AnyErrors)
        {
            cliLogger.Abort();
        }

        logger.Note("Module dump:\n\r" + proj.Module!.Value.PrintToString().ReplaceLineEndings());

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