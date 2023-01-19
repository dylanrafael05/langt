using Langt.AST;
using Langt.Optimization;
using Langt.Structure.Visitors;
using Langt.Utility;
using Langt.Utility.Collections;

namespace Langt.Codegen;

public class LangtProject
{
    public LangtProject(ILogger logger, string llvmModuleName)
    {
        Logger = logger;
        LLVMModuleName = llvmModuleName;

        logger.Init();

        CodeGenerator = new(this);
    }

    public CodeGenerator CodeGenerator {get;}
    public LangtScope GlobalScope {get;} = new(null);
    public List<LangtFile> Files {get;} = new();
    public DiagnosticCollection Diagnostics {get;} = new();
    public OrderedList<StaticReference> References {get;} = new();

    public ILogger Logger {get;}
    public string LLVMModuleName {get;}
    
    public LLVMModuleRef Module => CodeGenerator.Module;

    public void AddFileContents(FileInfo file)
        => AddFileContents(file.FullName, File.ReadAllText(file.FullName));
    public void AddFileContents(string name, string content)
    {
        if(Files.Any(f => f.Source.Name == name))
        {
            Logger.Fatal($"Attempting to add source {name} twice!");
        }
        
        Files.Add(new(this, new(content, name)));
    }

    private void HandleResult(IResultlike r) 
    {
        Diagnostics.AddResult(r);
        References.AddRange(r.GetBindingOptions().References);
    }

    public void BindSyntaxTrees()
    {
        var startState = GeneralPassState.Start(CodeGenerator);

        Logger.Note("Handling definitions . . . ");
        foreach(var f in Files)
        {
            CodeGenerator.Open(f);
            HandleResult(
                f.AST.HandleDefinitions(startState)
            );
        }

        Logger.Note("Refining definitions . . . ");
        foreach(var f in Files)
        {
            CodeGenerator.Open(f);
            HandleResult(
                f.AST.RefineDefinitions(startState)
            );
        }
        
        Logger.Note("Performing binding . . . ");
        foreach(var f in Files)
        {  
            CodeGenerator.Open(f);
            var r = f.AST.Bind(startState);  
            HandleResult(r);

            if(!r) continue;
            f.BoundAST = r.Value;
        }
    }

    public bool Compile(bool optimize) 
    {
#if DEBUG
        try
        {
#endif
        Logger.Note("Building . . . ");

        BindSyntaxTrees();

        if(Diagnostics.AnyErrors)
        {
            Logger.Note("Errors present! Stopping build process . . . ");
            return false;
        }


        Logger.Note("Lowering . . . ");
        foreach(var f in Files)
        {
            CodeGenerator.Open(f);
            f.BoundAST!.Lower(CodeGenerator);
        }

#if DEBUG
        Logger.Debug("Pre-optimization dump:\n\r" + CodeGenerator.Module.PrintToString().ReplaceLineEndings(), "llvm");
        if(!CodeGenerator.Verify()) return false;
#endif
        
        if(optimize)
        {
            Logger.Note("Optimizing . . . ");
            Optimizer.Optimize(CodeGenerator);

#if DEBUG
            if(!CodeGenerator.Verify()) return false;
#endif

            Logger.Note("Done building!");

            Logger.Debug("Post-optimization dump:\n\r" + CodeGenerator.Module.PrintToString().ReplaceLineEndings(), "llvm");
        }

        return true;

#if DEBUG
        }
        catch(Exception) 
        {
            LogAllDiagnostics();
            throw;
        }
#endif
    }

    public void LoadFromFileOrDirectory(string input) 
    {
        if(Directory.Exists(input))
        {
            var allFiles = Directory.EnumerateFiles(input, "*.lgt").ToList();

            if(allFiles.Count == 0)
            {
                Logger.Error("Provided directory has no langt files!");
            }

            foreach(var f in allFiles)
            {
                Logger.Note("Handling file " + Path.GetFullPath(f) + " . . . ");
                AddFileContents(new FileInfo(f));
            }
        }
        else
        {
            AddFileContents(new FileInfo(input));
        }
    }

    public void LogAllDiagnostics()
    {
        foreach(var d in Diagnostics)
        {
            Logger.Log(d.Severity, d.Message + " at " + d.Range);
        }
    }
}
