using Langt.AST;
using Langt.Structure;
using Langt.Structure.Collections;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt;

public class LangtProject
{
    public LangtProject(ILogger logger, string llvmModuleName)
    {
        Logger = logger;
        // LLVMModuleName = llvmModuleName;

        logger.Init();

        Context = new(this);
    }

    public Context Context {get;}
    public LangtScope GlobalScope {get;} = new(null);
    public List<LangtFile> Files {get;} = new();
    public DiagnosticCollection Diagnostics {get;} = new();
    public OrderedList<StaticReference> References {get;} = new();

    public ILogger Logger {get;}
    // public string LLVMModuleName {get;}
    
    // public LLVMModuleRef Module => Context.Module;

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
        var startState = GeneralPassState.Start(Context);

        Logger.Note("Handling definitions . . . ");
        foreach(var f in Files)
        {
            Context.Open(f);
            HandleResult(
                f.AST.HandleDefinitions(startState)
            );
        }

        Logger.Note("Refining definitions . . . ");
        foreach(var f in Files)
        {
            Context.Open(f);
            HandleResult(
                f.AST.RefineDefinitions(startState)
            );
        }
        
        Logger.Note("Performing binding . . . ");
        foreach(var f in Files)
        {  
            Context.Open(f);
            var r = f.AST.Bind(startState);  
            HandleResult(r);

            if(!r) continue;
            f.BoundAST = r.Value;
        }
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
