using Langt.Optimization;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.Codegen;

public class LangtProject
{
    public LangtProject(ILogger logger, string llvmModuleName)
    {
        Logger = logger;
        LLVMModuleName = llvmModuleName;
    }

    public LangtScope GlobalScope {get; private init;} = new();
    public List<LangtFile> Files {get; private init;} = new();
    public DiagnosticCollection Diagnostics {get; private init;} = new();

    public ILogger Logger {get; private init;}
    public string LLVMModuleName {get; init;}
    public LLVMModuleRef? Module {get; private set;}

    public void AddFileContents(FileInfo file)
        => AddFileContents(file.FullName, File.ReadAllText(file.FullName));
    public void AddFileContents(string name, string content)
    {
        if(Files.Any(f => f.Source.Name == name))
        {
            Logger.Error($"Attempting to add source {name} twice!");
        }
        
        Files.Add(new(this, new(content, name)));
    }

    public bool Build() 
    {
#if DEBUG
        try
        {
#endif
        var cg = new CodeGenerator(this);
        LLVMUtil.PrimeLLVM();

        Logger.Note("Building . . . ");
        
        Logger.Note("Initializing . . . ");
        foreach(var f in Files)
        {
            cg.Open(f);
            f.CompilationUnit.Initialize(cg);
        }

        Logger.Note("Defining types . . . ");
        foreach(var f in Files)
        {
            cg.Open(f);
            f.CompilationUnit.DefineTypes(cg);
        }

        Logger.Note("Implementing types . . . ");
        foreach(var f in Files)
        {
            cg.Open(f);
            f.CompilationUnit.ImplementTypes(cg);
        }
        
        Logger.Note("Defining functions . . . ");
        foreach(var f in Files)
        {
            cg.Open(f);
            f.CompilationUnit.DefineFunctions(cg);
        }
        
        Logger.Note("Type checking . . . ");
        foreach(var f in Files)
        {  
            cg.Open(f);
            f.CompilationUnit.TypeCheck(cg);
        }


        if(Diagnostics.AnyErrors)
        {
            Logger.Note("Errors present! Stopping build process . . . ");
            return false;
        }


        Logger.Note("Lowering . . . ");
        foreach(var f in Files)
        {
            cg.Open(f);
            f.CompilationUnit.Lower(cg);
        }

#if DEBUG
        Logger.Debug("Pre-optimization dump:\n\r" + cg.Module.PrintToString().ReplaceLineEndings(), "llvm");
        if(!cg.Verify()) return false;
#endif
        
        Logger.Note("Optimizing . . . ");
        Optimizer.Optimize(cg);

#if DEBUG
        if(!cg.Verify()) return false;
#endif

        Logger.Note("Done building!");
        Module = cg.Module;

        Logger.Debug("Post-optimization dump:\n\r" + cg.Module.PrintToString().ReplaceLineEndings(), "llvm");

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
