using System.Runtime.InteropServices;
using Langt.Lexing;
using Langt.Utility;
using Langt.Structure;
using Langt.Parsing;
using Langt.Structure.Visitors;
using Langt;
using System.CommandLine;
using System.Text.RegularExpressions;
using Langt.CG;
using Spectre.Console;
using System.CommandLine.Binding;

namespace Langt.CLI;

public enum InspectType
{
    BoundTree,
    SyntaxTree
}

public static partial class Program
{
    public static bool Noisy {get; private set;}
    public static bool Debug {get; private set;}

    public static RootCommand? Root {get; private set;}

    public const string NoOutputFile = "<derived from input>";

    public static async Task Main(string[] args)
    {
#if DEBUG
        if(args.Length == 0)
            args = Console.ReadLine()!.Split(" ");
#endif

        Root = new RootCommand("The compiler for the *Langt* language, a language created by [Dylan Rafael] in his spare time!");

            var noisy = new Option<bool>("--noisy", "Whether or not to log information while building");

                noisy.AddAlias("-n");
            
            Root.AddGlobalOption(noisy);

            var debugFlags = new Option<string[]>("--debug", "The flags to set debug for")
            {
                IsHidden = true,
                Arity = ArgumentArity.ZeroOrMore,
                AllowMultipleArgumentsPerToken = true
            };
            debugFlags.AddAlias("-d");
            Root.AddGlobalOption(debugFlags);

            var fileinput = new Argument<string>("input", "A path to the input file")
                .ValidFilePathOrDirectory(".txt", ".lgt");

            var disableopt = new Option<bool>("--no-opt", "Disables all optimization")
            {
                Arity = ArgumentArity.ZeroOrOne
            };
            disableopt.AddAlias("-O");

            var outputtype = new Option<string>("--otype", "The type of output to produce")
                .FromAmong("llvm");
            
            outputtype.SetDefaultValue("llvm");
            outputtype.AddAlias("-t");

            var outputname = new Option<string>("--out", "The name of the file to put the output in")
                .LegalFilePathsOnly();

            outputname.SetDefaultValue(NoOutputFile);
            outputname.AddAlias("-o");

            var run = new Command("run", """Runs the provided Langt file or directory through by means of its "main" function""")
            {
                fileinput,
                disableopt
            };

            run.SetHandler(OnRun, fileinput, disableopt, noisy, debugFlags);
            Root.Add(run);

            var compile = new Command("compile", """Compiles the given file or directory and outputs it in the given format""")
            {
                fileinput,
                disableopt,
                outputtype,
                outputname
            };
            compile.SetHandler(OnCompile, fileinput, disableopt, outputtype, outputname, noisy, debugFlags);
            Root.Add(compile);

            var inspectTy = new Option<InspectType>
            (
                "--type", 
                description: """The type of syntax tree to inspect""",

                parseArgument: r => 
                {
                    if(r.Tokens.Count != 1)
                    {
                        r.ErrorMessage = "Invalid amount of tokens for inspect type.";
                        return default;
                    }
                    else 
                    {
                        (r.ErrorMessage, var result) = r.Tokens[0].Value switch 
                        {
                            "syntax" or "s" => (null, InspectType.SyntaxTree),
                            "bind"   or "b" => (null, InspectType.BoundTree),

                            var v           => ($"Invalid inspection type {v}", default(InspectType))
                        };

                        return result;
                    }
                }
            )
            {
                ArgumentHelpName = "<syntax|s>|<bind|b>"
            };

            inspectTy.SetDefaultValue(InspectType.SyntaxTree);
            inspectTy.AddAlias("-t");

            var inspect = new Command("inspect", """Inspects the abstract syntax trees of a file""")
            {
                fileinput,
                inspectTy
            };
            
            inspect.SetHandler(OnInspect, fileinput, inspectTy, noisy, debugFlags);
            Root.Add(inspect);

            var loop = new Option<bool>("--loop", "Repeatedly await new arguments after each call");
            loop.AddAlias("-l");

            var defer = new Command("defer", """Defers command execution to read arguments from the console input. Ignores all options.""")
            {
                loop
            };

            defer.SetHandler(DeferAsync, loop);
            Root.Add(defer);

        Root.TreatUnmatchedTokensAsErrors = true;
        await Root.InvokeAsync(args);
    }

    public static void SetFlags(bool noisy)
    {
        Noisy = noisy;
    }

    public async static Task DeferAsync(bool loop) 
    {   
        do
        {
            Console.Clear();

            Console.WriteLine("Welcome to . . .");
            Console.WriteLine(".-----------------------.");
            Console.WriteLine("|     DEFFERED JAIL     |");
            Console.WriteLine("'-----------------------'");
            Console.WriteLine();
            Console.WriteLine("Enter arguments (omitting the call to langt):");

            var line = Console.ReadLine()!;
            var args = Whitespace().Split(line);

            await Root!.InvokeAsync(args);

            if(loop) 
            {
                Console.WriteLine("Done! Press any key to continue, and escape to exit . . . ");
                var k = Console.ReadKey().Key;

                if(k == ConsoleKey.Escape) break;
            }
        }
        while(loop);
    }

    public static LangtProject BuildProject(string input, ILogger logger)
    {
        var proj = new LangtProject(logger);
        proj.LoadFromFileOrDirectory(input);
        proj.BindSyntaxTrees();

        return proj;
    }

    public static LangtCompilation BuildCompilation(string input, ILogger logger, bool disableopt)
    {
        var comp = new LangtCompilation(BuildProject(input, logger), input);
        comp.Build(!disableopt);

        comp.Project.LogAllDiagnostics();

        if(comp.Diagnostics.AnyErrors)
        {
            CLILogger.Abort();
        }

        return comp;
    }

    public static ILogger InitLogger(bool noisy, string[] debugFlags)
    {
        SetFlags(noisy);

        return new CLILogger()
        {
            DebugFlags = debugFlags.ToHashSet()
        };
    }

    public static bool Try(Action a) 
    {
        try
        {
            a();

            return true;
        }
        catch(Exception e) 
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("An error occured: " + e.Message);

#if DEBUG
            Console.WriteLine("Press enter to continue, press space to see error stack.");

            if(Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                Console.WriteLine();
                Console.Write(e.ToString());
            }
#endif

            Console.ResetColor();
            return false;
        }
    }

    public static void OnCompile(string input, bool disableopt, string otype, string ofilename, bool noisy, string[] debugFlags) => Try(() =>
    {
        Console.WriteLine();

        using ILogger logger = InitLogger(noisy, debugFlags);
        var proj = BuildCompilation(input, logger, disableopt);

        if(ofilename == NoOutputFile) ofilename = Path.ChangeExtension(input, "ll");

        proj.Module.PrintToFile(ofilename);
    });

    public static void OnInspect(string input, InspectType type, bool noisy, string[] debugFlags) => Try(() =>
    {
        Console.WriteLine();

        using ILogger logger = InitLogger(noisy, debugFlags);
        var proj = BuildProject(input, logger);

        var f = proj.Files.First();
        var r = (ITreeRenderable)(type switch 
        {
            InspectType.BoundTree  => f.BoundAST!,
            InspectType.SyntaxTree => f.AST!,

            _ => throw new NotSupportedException($"Unknown inspection type {type}")
        });

        AnsiConsole.Write(r.ToStringTree().Build(Style.Parse("gray"), TreeGuide.Line));
    });

    public static void OnRun(string input, bool disableopt, bool noisy, string[] debugFlags) => Try(() =>
    {
        Console.WriteLine();

        using ILogger logger = InitLogger(noisy, debugFlags);
        var proj = BuildCompilation(input, logger, disableopt);

        logger.Note("Running function main in " + input + " . . .");

        LLVMUtil.CallMain(proj.Module, logger);
        
        Console.WriteLine();
        logger.Note("Finished running!");
        Console.WriteLine();
    });

    [GeneratedRegex(@"[ \t]+")]
    private static partial Regex Whitespace();
}