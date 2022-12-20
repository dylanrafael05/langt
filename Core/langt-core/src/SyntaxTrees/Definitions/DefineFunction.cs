using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ArgumentSpec(ASTToken Name, ASTType Type) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Name, Type};
}
public record VarargSpec(ASTToken Ellipsis) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Ellipsis};
}

public record DefineFunction(ASTToken Let,
                             ASTToken Identifier,
                             ASTToken Open,
                             SeparatedCollection<ArgumentSpec> ArgSpec,
                             VarargSpec? VarargSpec,
                             ASTToken Close,
                             ASTType Type,
                             FunctionBody? Body) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Let, Identifier, Open, ArgSpec, VarargSpec, Close, Type, Body};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Defining function " + Identifier.Range.Content.ToString());
        foreach(var argspec in ArgSpec.Values)
        {
            // TODO: implement depth modification by parameters; headers
            visitor.PutString(argspec.Name.ContentStr);
            visitor.Visit(argspec.Type);
        }

        if(Body is not null) visitor.Visit(Body);
    }

    public LangtScope? Scope {get; private set;}
    public LangtFunctionGroup? FunctionGroup {get; private set;}
    public LangtFunction? Function {get; private set;}
    public LangtFunctionType? FunctionType {get; private set;}

    public override void DefineFunctions(ASTPassState state)
    {
        var retType = Type.Resolve(state);
        if(retType is null) return;

        var argTypes = new LangtType[ArgSpec.Values.Count()];
        
        var argc = 0;
        foreach(var a in ArgSpec.Values.Select(s => s.Type.Resolve(state)))
        {
            if(a is null) return;
            argTypes[argc++] = a;
        }

        FunctionType = new LangtFunctionType(retType, VarargSpec is not null, argTypes);
        var lftype = state.CG.LowerType(FunctionType);

        var lf = state.CG.Module.AddFunction(
            CodeGenerator.GetGeneratedFunctionName(
                Let.Type is TokenType.Extern, 
                state.CG.CurrentNamespace, 
                Identifier.ContentStr,
                FunctionType.IsVararg,
                FunctionType.ParameterTypes
            ), 
            lftype
        );

        // TODO: redo this to use better functions
        FunctionGroup = state.CG.ResolutionScope.ResolveFunctionGroup(
            Identifier.ContentStr, 
            Range, 
            state,
            propogate: false
        );

        if(FunctionGroup is null)
        {
            FunctionGroup = new(Identifier.ContentStr);
            state.CG.ResolutionScope.DefineFunctionGroup(FunctionGroup, Range, state); 
        }
        
        Function = new(
            FunctionType, 
            ArgSpec.Values.Select(v => v.Name.ContentStr).ToArray(), 
            lf
        );

        FunctionGroup.AddFunctionOverload(Function, this, state);
        
        RawExpressionType = LangtType.None;
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        if(Let.Type is TokenType.Extern) return; //TODO: precheck extern existence

        if(FunctionType is null) return; // Previous error occurred.

        var previousFunction = state.CG.CurrentFunction;

        state.CG.CurrentFunction = Function;
        Scope = state.CG.CreateUnnamedScope();

        var count = 0u;
        foreach(var argspec in ArgSpec.Values)
        {
            var t = argspec.Type.Resolve(state);

            if(t is null) continue;

            Scope.ForceDefineVariable(new(argspec.Name.ContentStr, t)
            {
                ParameterNumber = count
            });

            count++;
        }

        Body!.TypeCheck(state);

        if(Body is FunctionExpressionBody exp)
        {
            if(!state.MakeMatch(FunctionType!.ReturnType, exp.Expression))
            {
                state.Error($"Function must return {exp.Expression.TransformedType.Name}, but instead returns {exp.Expression.TransformedType.Name}", exp.Range);
            }
        }
        else if(Body is FunctionBlockBody blk)
        {
            if(!blk.Returns)
            {
                if(FunctionType!.ReturnType == LangtType.None)
                {
                    state.CG.Builder.BuildRetVoid();
                }
                else 
                {
                    state.Error($"Function must return a value of type {FunctionType!.ReturnType.Name}", blk.Range);
                }
            }
        }

        state.CG.CloseScope();
        state.CG.CurrentFunction = previousFunction;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(Let.Type is TokenType.Extern) return; //work is already done for us when creating the prototypes!
        
        var bb = lowerer.LLVMContext.AppendBasicBlock(Function!.LLVMFunction, "entry");
        lowerer.Builder.PositionAtEnd(bb);

        lowerer.CurrentFunction = Function;

        foreach(var variable in Scope!.NamedItems.Values.Where(t => t is LangtVariable).Cast<LangtVariable>())
        {
            var v = lowerer.Builder.BuildAlloca(lowerer.LowerType(variable.Type), "var."+variable.Name);
            
            if(variable.IsParameter)
            {
                lowerer.Builder.BuildStore(Function.LLVMFunction.GetParam(variable.ParameterNumber!.Value), v);
            }

            variable.Attach(v);
        }

        if(Body is FunctionExpressionBody exp)
        {
            exp.Lower(lowerer);
            lowerer.Builder.BuildRet(lowerer.PopValue(DebugSourceName).LLVM);
        }
        else if(Body is FunctionBlockBody blk)
        {
            blk.Lower(lowerer);
            if(FunctionType!.ReturnType == LangtType.None)
            {
                lowerer.Builder.BuildRetVoid();
            }
        }
        else throw new Exception("Unknown function body type " + Body!.GetType().Name);
    }
}