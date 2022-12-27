using Langt.AST;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.Codegen;

public class CodeGenerator : IProjectDependency
{
    public const string LangtIdentifierPrepend = "<langt>::";

    public LangtProject Project {get; init;}

    public LLVMContextRef LLVMContext {get; private init;}
    public LLVMModuleRef Module {get; private init;}
    public LLVMBuilderRef Builder {get; private init;}

    public LangtFunction? CurrentFunction {get; set;}
    /// <summary>
    /// The scope in which resolution is currently taking place.
    /// </summary>
    public LangtScope ResolutionScope {get; private set;}
    /// <summary>
    /// The scope that the AST is currently inside of.
    /// </summary>
    /// <value></value>
    public LangtScope CurrentScope {get; private set;}
    /// <summary>
    /// Whether or not the resolution scope is the same as the current scope.
    /// </summary>
    public bool ResolutionScopeIsRedirected {get; private set;}
    /// <summary>
    /// The current file this code generator is working with.
    /// </summary>
    public LangtFile? CurrentFile {get; private set;}
    /// <summary>
    /// The current namespace as declared by 'namespace' directives.
    /// Will be null if in the global namespace or uninitialized/unrun.
    /// </summary>
    public LangtNamespace? CurrentNamespace {get; private set;}

    /// <summary>
    /// Open the given langt file with this code generator.
    /// </summary>
    /// <param name="file">The file to open.</param>
    public void Open(LangtFile file) 
    {
        CurrentFile = file;
        ResolutionScope = CurrentScope = file.Scope;
        CurrentNamespace = null;
    }

    private readonly List<LangtConversion> conversions = new();
    private readonly Dictionary<(LangtType to, LangtType from), LangtConversion> directConversions = new();

    private readonly Dictionary<LangtType, LLVMTypeRef> loweredTypes = new();
    private readonly Stack<LangtValue> unnamedValues = new();
    
    private readonly Dictionary<string, LLVMValueRef> llvmIntrinsics = new();

    /// <summary>
    /// The diagnostic collector used by the project this code generator depends on.
    /// </summary>
    public DiagnosticCollection Diagnostics => Project.Diagnostics;
    /// <summary>
    /// The logger used by the project this code generator depends on.
    /// </summary>
    public ILogger Logger => Project.Logger;

    public CodeGenerator(LangtProject project)
    {
        Project = project;

        LLVMContext = LLVMContextRef.Global;
        Module      = LLVMContext.CreateModuleWithName(project.LLVMModuleName);

        Builder = LLVMContext.CreateBuilder();
        
        ResolutionScope = CurrentScope = project.GlobalScope;
        CurrentFile = null;

        Initialize();
    }

    public void Initialize()
    {
        // TYPES //
        foreach(var field in typeof(LangtType).GetFields().Where(f => Attribute.IsDefined(f, typeof(BuiltinTypeAttribute))))
        { 
            Project.GlobalScope.DefineType((LangtType)field.GetValue(null)!, SourceRange.Default).Expect();
        }
        
        foreach(var conv in LangtConversion.Builtin)
        {
            DefineConversion(conv);
        }
    }

    public void SetCurrentNamespace(LangtNamespace ns)
    {
        CurrentFile!.Scope.HoldingScope = ns;
        CurrentNamespace = ns;
    }

    public void DefineConversion(LangtConversion conversion)
    {
        if(conversion.TransformProvider.IsDirect)
        {
            directConversions.Add((conversion.TransformProvider.DirectResult!, conversion.TransformProvider.DirectInput!), conversion);
        }
        else
        {
            conversions.Add(conversion);
        }
    }

    public LangtScope CreateUnnamedScope()
        => ResolutionScope = ResolutionScope.AddUnnamedScope();
    public void CloseScope()
    {
        ResolutionScope = ResolutionScope.HoldingScope ?? throw new Exception("Cannot close a scope; the current scope is the global scope!");
    }


    // TODO: reduce value smuggling by clearing discarded values and removing 'none' values
    public void PushValueNoDebug(LangtValue value)
        => unnamedValues.Push(value);
    public void PushValue(LangtType type, LLVMValueRef value, string debugSource)
    {
        Logger.Debug($"\tProduced one value from {debugSource}: type {type.GetFullName()}, value {value.Name}", "lowering");
        unnamedValues.Push(new(type, value));
    }

    public LangtValue PopValue(string source)
    {
        var s = PopValueNoDebug();
        Project.Logger.Debug($"     Consumed one value from {source}; type {s.Type.Name}, value {s.LLVM.Name}", "lowering");
        return s;
    }
    public LangtValue PopValueNoDebug()
    {
        return unnamedValues.Pop();
    }
    
    public void DiscardValues(string debugSource)
    {
        Logger.Debug($"Clearing stack from {debugSource}", "lowering");
        unnamedValues.Clear();
    }

    public void LogStack(string source)
    {
        var res = "     Stack after " + source + "";

        if(unnamedValues.Count == 0)
        {
            res += " is empty";
        }
        else foreach(var s in unnamedValues)
        {
            res += "\r\n          : type " + s.Type.Name + ", value " + s.LLVM.Name;
        }

        Logger.Debug(res, "lowering");
    }

    public bool Verify()
    {
        if(!Module.TryVerify(LLVMVerifierFailureAction.LLVMReturnStatusAction, out var msg))
        {
            Project.Logger.Fatal(msg.ReplaceLineEndings());
            return false;
        }

        return true;
    }

    public LLVMTypeRef LowerType(LangtType type)
        => loweredTypes.TryGetValue(type, out var res) 
            ? res 
            : loweredTypes[type] = type.Lower(this);

    public Result<LangtConversion> ResolveConversion(LangtType to, LangtType from, SourceRange range) 
    {
        if(!directConversions.TryGetValue((to, from), out var conv))
        {
            conv = conversions.FirstOrDefault(c => c.TransformProvider.CanPerform(from, to));

            if(conv is null)
            {
                return ResultBuilder.Empty()
                    .WithDgnError($"Could not find a conversion from {from.GetFullName()} to {to.GetFullName()}", range)
                    .Build<LangtConversion>()
                ;
            }
        }
        
        return Result.Success(conv);
    }

    public LLVMValueRef GetIntrinsic(string name, LangtFunctionType functionType)
    {
        if(!llvmIntrinsics.TryGetValue(name, out var v))
        {
            var nv = Module.AddFunction(name, LowerType(functionType));

            llvmIntrinsics.Add(name, nv);
            
            return nv;
        }
        else
        {
            return v;
        }
    }

    public static string GetGeneratedFunctionName(bool isExtern, LangtNamespace? currentNamespace, string name, bool isVararg, params LangtType[] paramTypes)
    {
        if(isExtern) return name;
        return LangtIdentifierPrepend 
            + (currentNamespace is null ? "" : currentNamespace.GetFullName() + "::") 
            + name 
            + $"({LangtFunctionType.GetSignatureString(isVararg, paramTypes)})";
    }
}