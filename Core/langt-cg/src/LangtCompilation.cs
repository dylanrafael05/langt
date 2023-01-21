using Langt.CG.Optimization;
using Langt.Structure;

namespace Langt.CG;

public class LangtCompilation
{
    public LangtCompilation(LangtProject project, string llvmModName)
    {
        Project = project;
        LLVMModuleName = llvmModName;

        Generator = new(this);
    }

    public LangtProject Project {get; init;}
    public CodeGenerator Generator {get; init;}
    public string LLVMModuleName {get;}

    public LLVMModuleRef Module => Generator.Module;
    public ILogger Logger => Project.Logger;
    public DiagnosticCollection Diagnostics => Project.Diagnostics;

    public bool Compile(bool optimize)
    {
        Logger.Note("Beginning . . . ");
        Project.BindSyntaxTrees();

        if(Diagnostics.AnyErrors)
        {
            Logger.Note("Errors present! Stopping build process . . . ");
            return false;
        }

        return Build(optimize);
    }

    public bool Build(bool optimize) 
    {
#if DEBUG
        try
        {
#endif

        Logger.Note("Lowering . . . ");
        foreach(var f in Project.Files)
        {
            Generator.Lower(f.BoundAST!);
        }

#if DEBUG
        Logger.Debug("Pre-optimization dump:\n\r" + Generator.Module.PrintToString().ReplaceLineEndings(), "llvm");
        if(!Generator.Verify()) return false;
#endif
        
        if(optimize)
        {
            Logger.Note("Optimizing . . . ");
            Optimizer.Optimize(Generator);

#if DEBUG
            if(!Generator.Verify()) return false;
#endif

            Logger.Note("Done building!");

            Logger.Debug("Post-optimization dump:\n\r" + Generator.Module.PrintToString().ReplaceLineEndings(), "llvm");
        }

        return true;

#if DEBUG
        }
        catch(Exception) 
        {
            Project.LogAllDiagnostics();
            throw;
        }
#endif
    }
}
