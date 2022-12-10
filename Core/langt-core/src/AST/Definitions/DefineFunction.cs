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

    public override void DefineFunctions(CodeGenerator generator)
    {
        var retType = Type.Resolve(generator);
        if(retType is null) return;

        var argTypes = new LangtType[ArgSpec.Values.Count()];
        
        var argc = 0;
        foreach(var a in ArgSpec.Values.Select(s => s.Type.Resolve(generator)))
        {
            if(a is null) return;
            argTypes[argc++] = a;
        }

        FunctionType = new LangtFunctionType(retType, VarargSpec is not null, argTypes);
        var lftype = generator.LowerType(FunctionType);

        var lf = generator.Module.AddFunction(
            CodeGenerator.GetGeneratedFunctionName(
                Let.Type is TokenType.Extern, 
                generator.CurrentNamespace, 
                Identifier.ContentStr,
                FunctionType.IsVararg,
                FunctionType.ParameterTypes
            ), 
            lftype
        );

        // TODO: redo this to use better functions
        FunctionGroup = generator.ResolutionScope.ResolveFunctionGroup(
            Identifier.ContentStr, 
            Range, 
            generator.Diagnostics,
            err: false,
            propogate: false
        );

        if(FunctionGroup is null)
        {
            FunctionGroup = new(Identifier.ContentStr);
            generator.ResolutionScope.DefineFunctionGroup(FunctionGroup, Range, generator.Diagnostics); 
        }
        
        Function = new(
            FunctionType, 
            ArgSpec.Values.Select(v => v.Name.ContentStr).ToArray(), 
            lf
        );

        FunctionGroup.AddFunctionOverload(Function, Range, generator.Diagnostics);
        
        ExpressionType = LangtType.None;
        
        // TODO: more rigorous resolution here
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        if(Let.Type is TokenType.Extern) return; //TODO: precheck extern existence

        if(FunctionType is null) return; // Previous error occurred.

        var previousFunction = generator.CurrentFunction;

        generator.CurrentFunction = Function;
        Scope = generator.CreateUnnamedScope();

        var count = 0u;
        foreach(var argspec in ArgSpec.Values)
        {
            var t = argspec.Type.Resolve(generator);

            if(t is null) continue;

            Scope.ForceDefineVariable(new(argspec.Name.ContentStr, t)
            {
                ParameterNumber = count
            });

            count++;
        }

        Body!.TypeCheck(generator);

        if(Body is FunctionExpressionBody exp)
        {
            if(!generator.MakeMatch(FunctionType!.ReturnType, exp.Expression))
            {
                generator.Diagnostics.Error($"Function must return {exp.Expression.TransformedType.Name}, but instead returns {exp.Expression.TransformedType.Name}", exp.Expression.Range);
            }
        }
        else if(Body is FunctionBlockBody blk)
        {
            if(!blk.Returns)
            {
                if(FunctionType!.ReturnType == LangtType.None)
                {
                    generator.Builder.BuildRetVoid();
                }
                else 
                {
                    generator.Diagnostics.Error($"Function must return a value of type {FunctionType!.ReturnType.Name}", Range);
                }
            }
        }

        generator.CloseScope();
        generator.CurrentFunction = previousFunction;
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
            exp.LowerSelf(lowerer);
            lowerer.Builder.BuildRet(lowerer.PopValue().LLVM);
        }
        else if(Body is FunctionBlockBody blk)
        {
            blk.LowerSelf(lowerer);
            if(FunctionType!.ReturnType == LangtType.None)
            {
                lowerer.Builder.BuildRetVoid();
            }
        }
        else throw new Exception("Unknown function body type " + Body!.GetType().Name);
    }
}