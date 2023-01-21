using Langt.AST;
using Langt.Structure;
using Langt.Lexing;
using Langt.Parsing;
using Langt.CG.Bindings;
using Langt.CG.Structure;
using Langt.CG.Lowering;

namespace Langt.CG;

public class CodeGenerator
{
    public const string LangtIdentifierPrepend = "<langt>";

    public CodeGenerator(LangtCompilation compilation)
    {
        Compilation = compilation;
        TypeBuilder = new LangtTypeBuilder {CG = this};
        Binder = new(TypeBuilder);

        Logger = compilation.Logger;

        LLVMContext = LLVMContextRef.Global;   
        Module  = LLVMContext.CreateModuleWithName(compilation.LLVMModuleName);

        Target = LLVMTargetDataRef.FromStringRepresentation(Module.DataLayout);

        Builder = LLVMContext.CreateBuilder();

        Initialize();
    }

    public void Initialize()
    {
        BuiltinOperators.Initialize(this);
    }

    public LangtCompilation Compilation {get;}
    public TypeBuilder TypeBuilder {get;}
    public Binder Binder {get;}

    public LangtFunction? CurrentFunction {get; set;}

    public LLVMContextRef LLVMContext {get;}
    public LLVMTargetDataRef Target {get;}
    public LLVMModuleRef Module {get;}
    public LLVMBuilderRef Builder {get;}

    public ILogger Logger {get;}
    
    private readonly Dictionary<LangtType, LLVMTypeRef> loweredTypes = new();
    private readonly Stack<LangtValue> unnamedValues = new();
    
    private readonly Dictionary<string, LLVMValueRef> llvmIntrinsics = new();

    public delegate LLVMValueRef BinaryOpDefiner(LLVMBuilderRef builder, LLVMValueRef a, LLVMValueRef b);
    public delegate LLVMValueRef UnaryOpDefiner(LLVMBuilderRef builder, LLVMValueRef x);

    public void Lower(BoundASTNode node) 
    {
        Logger.Debug("Lowering " + node.DebugSourceName, "lowering");

        if(node.Unreachable)
        {
            return;
        }

        ILowerImplementation.Lower(this, node);
    }

    public ulong Sizeof(LangtType type)
    {
        if(type == LangtType.None) return 0;

        var l = Binder.Get(type);
        return Target.StoreSizeOfType(l);
    }

    public void DefineUnaryOperator(TokenType op, LangtType x, LangtType r, UnaryOpDefiner definer)
    {
        var opfn = Compilation.Project.Context.GetOperator(new(OperatorType.Unary, op));

        var ftype = LangtFunctionType.Create(new[] {x}, r).Expect();

        var lfn = CreateNewLLVMFunction(opfn.Name, false, ftype);
        var fn = new LangtFunction(opfn)
        {
            Type           = ftype,
            ParameterNames = new[] {"__x"},
            LLVMFunction   = lfn
        };

        opfn.AddFunctionOverload(fn, SourceRange.Default).Expect("Cannot redefine operator");

        var bb = LLVMContext.AppendBasicBlock(fn.LLVMFunction, "entry");
        Builder.PositionAtEnd(bb);

        Builder.BuildRet
        (
            definer(Builder, lfn.GetParam(0))
        );
    }

    public void DefineBinaryOperator(TokenType op, LangtType a, LangtType b, LangtType r, BinaryOpDefiner definer)
    {
        // Logger.Note($"Defining op {a.FullName} {op} {b.FullName}");
        
        var opfn = Compilation.Project.Context.GetOperator(new(OperatorType.Binary, op));

        var ftype = LangtFunctionType.Create(new[] {a, b}, r).Expect();

        var lfn = CreateNewLLVMFunction(opfn.Name, false, ftype);
        var fn = new LangtFunction(opfn)
        { 
            Type           = ftype,
            ParameterNames = new[] {"__a", "__b"},
            LLVMFunction   = lfn
        };

        opfn.AddFunctionOverload(fn, SourceRange.Default).Expect("Cannot redefine operator");

        var bb = LLVMContext.AppendBasicBlock(fn.LLVMFunction, "entry");
        Builder.PositionAtEnd(bb);

        Builder.BuildRet
        (
            definer(Builder, lfn.GetParam(0), lfn.GetParam(1))
        );
    }

    public void BuildFunctionCall(LLVMValueRef fn, BoundASTNode[] arguments, LangtFunctionType fntype, string sourceName)
    {
        var llvmArgs = new LLVMValueRef[arguments.Length];

        for(var i = 0; i < arguments.Length; i++)
        {
            Lower(arguments[i]);
            llvmArgs[i] = PopValue(sourceName).LLVM;
        }

        var r = Builder.BuildCall2(
            Binder.Get(fntype!),
            fn, 
            llvmArgs
        );

        if(fntype!.ReturnType != LangtType.None)
        {
            PushValue(fntype!.ReturnType, r, sourceName);
        }
    }

    public LLVMValueRef CreateNewLLVMFunction(string name, bool isExtern, LangtFunctionType type, LangtNamespace ns) 
    {
        return Module.AddFunction
        (
            GetGeneratedFunctionName
            (
                isExtern, 
                ns, 
                name,
                type.IsVararg,
                type.ParameterTypes
            ), 
            Binder.Get(type)
        );
    }

    public void PushValueNoDebug(LangtValue value)
        => unnamedValues.Push(value);
    public void PushValue(LangtType type, LLVMValueRef value, string debugSource)
    {
        Logger.Debug($"\tProduced one value from {debugSource}: type {type.FullName}, value {value.Name}", "lowering");
        unnamedValues.Push(new(type, value));
    }

    public LangtValue PopValue(string source)
    {
        var s = PopValueNoDebug();
        Logger.Debug($"     Consumed one value from {source}; type {s.Type.Name}, value {s.LLVM.Name}", "lowering");
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
            Logger.Fatal(msg.ReplaceLineEndings());
            return false;
        }

        return true;
    }

    public LLVMValueRef GetIntrinsic(string name, LangtFunctionType functionType)
    {
        if(!llvmIntrinsics.TryGetValue(name, out var v))
        {
            var nv = Module.AddFunction(name, Binder.Get(functionType));

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
            + (currentNamespace is null ? "" : currentNamespace.FullName + "::") 
            + name 
            + LangtFunctionType.GetFullSignatureString(isVararg, paramTypes);
    }
}