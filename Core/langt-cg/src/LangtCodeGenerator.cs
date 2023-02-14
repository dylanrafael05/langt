﻿using Langt.AST;
using Langt.Structure;
using Langt.Lexing;
using Langt.Parsing;
using Langt.CG.Bindings;
using Langt.CG.Structure;
using Langt.CG.Lowering;
using Langt.Structure.Resolutions;

namespace Langt.CG;

public class CodeGenerator
{
    public delegate LLVMValueRef Applicator(LLVMValueRef input);

    public CodeGenerator(LangtCompilation compilation)
    {
        Compilation = compilation;

        TypeBuilder = new TypeBuilder       {CG = this};
        FuncBuilder = new FunctionBuilder   {CG = this};
        ConvBuilder = new ConversionBuilder {CG = this};

        Binder = new(TypeBuilder, FuncBuilder, ConvBuilder);

        LLVMContext = LLVMContextRef.Global;   
        Module      = LLVMContext.CreateModuleWithName(compilation.LLVMModuleName);

        Target = LLVMTargetDataRef.FromStringRepresentation(Module.DataLayout);

        Builder = LLVMContext.CreateBuilder();

        ImplementBuiltinOperators.Initialize(this);
    }

    public LangtCompilation Compilation {get;}
    public Builder<LangtType, LLVMTypeRef> TypeBuilder {get;}
    public Builder<LangtFunction, LLVMValueRef> FuncBuilder {get;}
    public Builder<LangtConversion, Applicator> ConvBuilder {get;}
    public Binder Binder {get;}

    public LLVMValueRef CurrentFunction {get; private set;} = default;
    public LangtFunctionType CurrentFunctionType {get; private set;} = default!;

    public LLVMContextRef LLVMContext {get;}
    public LLVMTargetDataRef Target {get;}
    public LLVMModuleRef Module {get;}
    public LLVMBuilderRef Builder {get;}

    public ILogger Logger => Compilation.Logger;
    public LangtProject Project => Compilation.Project;
    public Context Context => Project.Context;
    
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

    // TODO: make static specs available
    public LangtFunctionGroup GetOperator(OperatorSpec spec) 
        => Compilation.Project.Context.GetOperator(spec);

    public void EnterFunction(LangtFunction fn) 
    {
        var l = Binder.Get(fn);
        CurrentFunction = l;
        CurrentFunctionType = fn.Type;

        var entry = LLVMContext.AppendBasicBlock(l, ".entry");
        Builder.PositionAtEnd(entry);
    }
    public void ExitFunction()
    {
        CurrentFunction = default;
        CurrentFunctionType = default!;
    }
    public void Function(LangtFunction fn, Action act)
    {
        EnterFunction(fn);
        act();
        ExitFunction();
    }

    public void InitializeScope(IScope scope)
    {
        foreach(var variable in scope!.Items.Values.OfType<LangtVariable>())
        {
            var llvm = Builder.BuildAlloca(Binder.Get(variable.Type), "var."+variable.Name);
            
            if(variable.IsParameter)
            {
                Builder.BuildStore
                (
                    CurrentFunction!.GetParam(variable.ParameterNumber!.Value), 
                    llvm
                );
            }

            Binder.BindVariable(variable, llvm);
        }
    }

    public int SizeofIntInBits(LangtType intTy) => intTy.Name switch 
    {
        LangtWords.Integer8  or LangtWords.UnsignedInteger8  => 8,
        LangtWords.Integer16 or LangtWords.UnsignedInteger16 => 16,
        LangtWords.Integer32 or LangtWords.UnsignedInteger32 => 32,
        LangtWords.Integer64 or LangtWords.UnsignedInteger64 => 64,
        LangtWords.IntegerN  or LangtWords.UnsignedIntegerN  => (int)Target.SizeOfTypeInBits(Binder.Get(intTy)),

        _ => throw new NotSupportedException($"Unexpected type {intTy} supplied to {nameof(SizeofIntInBits)}")
    };

    public ulong Sizeof(LangtType type)
    {
        if(type == LangtType.None) return 0;

        var l = Binder.Get(type);
        return Target.StoreSizeOfType(l);
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

    public void PushValueNoDebug(LangtValue value)
        => unnamedValues.Push(value);
    public void PushValue(LangtValue val, string debugSource)
    {
        Logger.Debug($"\tProduced one value from {debugSource}: type {val.Type.FullName}, value {val.LLVM.Name}", "lowering");
        unnamedValues.Push(val);
    }
    public void PushValue(LangtType ty, LLVMValueRef val, string debugSource)
        => PushValue(new(ty, val), debugSource);

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
}