using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record ArgumentSpec(ASTToken Name, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Name, Type};
}
public record VarargSpec(ASTToken Ellipsis) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Ellipsis};
}

public record BoundFunctionDefinition(FunctionDefinition Source,
                                      LangtFunction Function,
                                      BoundASTNode Body) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Body};

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(Source.Let.Type is TokenType.Extern) return; //work is already done for us when creating the prototypes!
        
        lowerer.BuildFunction(Function, Body);
    }
}

public record FunctionDefinition(ASTToken Let,
                             ASTToken Identifier,
                             ASTToken Open,
                             SeparatedCollection<ArgumentSpec> ArgSpec,
                             VarargSpec? VarargSpec,
                             ASTToken Close,
                             ASTType Type,
                             FunctionBody? Body) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Let, Identifier, Open, ArgSpec, VarargSpec, Close, Type, Body};

    public SourceRange DefinitionRange = SourceRange.CombineFrom(Let, Close);

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

    private LangtFunctionGroup? FunctionGroup {get; set;}
    private LangtFunction? Function {get; set;}
    private LangtType?[]? ArgTypes {get; set;}

    public override Result HandleDefinitions(ASTPassState state)
    {
        FunctionGroup = state.CG.ResolutionScope.ResolveFunctionGroup
            (
                Identifier.ContentStr, 
                Range,
                propogate: false
            )
            .OrDefault()
        ;

        if(FunctionGroup is null)
        {
            return state.CG.ResolutionScope.Define
            (
                s => new LangtFunctionGroup(Identifier.ContentStr, s), Range, 
                f => FunctionGroup = f
            ); 
        }
        else
        {
            return Result.Success();
        }
    }

    public override Result RefineDefinitions(ASTPassState state)
    {
        state.CG.Logger.Debug($"Entered .{nameof(RefineDefinitions)}", "results");
        var builder = ResultBuilder.Empty();

        var rtr = Type.Resolve(state);
        builder.AddData(rtr);

        if(!rtr) return builder.Build();

        var retType = rtr.Value;

        ArgTypes = new LangtType[ArgSpec.Values.Count()];
        
        var argc = 0;
        foreach(var a in ArgSpec.Values.Select(s => s.Type.Resolve(state)))
        {
            builder.AddData(a);

            if(!a) return builder.Build();

            ArgTypes[argc] = a.Value;

            argc++;
        }

        var fnres = LangtFunctionType.Create
        (
            ArgTypes!, 
            retType, 
            range: DefinitionRange,
            parameterNames: ArgSpec.Values.Select(k => k.Name.ContentStr).ToArray(),
            isVararg: VarargSpec is not null,
            externType: (Let.Type is TokenType.Extern ? "C" : "")
        );
        builder.AddData(fnres);

        if(!builder) return builder.Build();

        var fnType = fnres.Value;

        var lf = state.CG.CreateNewLLVMFunction(Identifier.ContentStr, Let.Type is TokenType.Extern, fnType);
        
        Function = new LangtFunction(FunctionGroup!)
        {
            Type            = fnType,
            ParameterNames  = ArgSpec.Values.Select(v => v.Name.ContentStr).ToArray(),
            LLVMFunction    = lf,

            Documentation   = Let.Documentation,
            DefinitionRange = SourceRange.CombineFrom(Let, Type)
        };

        var afr = FunctionGroup!.AddFunctionOverload(Function, Range);
        builder.AddData(afr);

        builder.AddStaticReference(Identifier.Range, Function, true);

        return builder.Build();
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        if(Let.Type is TokenType.Extern || Function is null)
        {
            // TODO: precheck extern existence
            return Result.Success<BoundASTNode>(new BoundASTWrapper(this));
        }

        var previousFunction = state.CG.CurrentFunction;
        
        Result<BoundASTNode> bodyResult;

        state.CG.CurrentFunction = Function;
        var scope = state.CG.OpenScope();
        {
            DefineParametersInScope(builder, scope);

            if(Function.Type.ReturnType == LangtType.None || Body is FunctionBlockBody)
            {
                bodyResult = Body!.Bind(state, new TypeCheckOptions {PredefinedBlockScope = scope});
            }
            else 
            {
                bodyResult = Body!.Bind(state, new TypeCheckOptions {TargetType = Function.Type.ReturnType});
            }

            if(!builder.WithData(bodyResult)) return builder.BuildError<BoundASTNode>();
        }
        state.CG.CloseScope();
        state.CG.CurrentFunction = previousFunction;

        return builder.Build<BoundASTNode>
        (
            new BoundFunctionDefinition(this, Function, bodyResult.Value)  
        );
    }

    private void DefineParametersInScope(ResultBuilder builder, IScope scope)
    {
        var count = 0u;
        foreach(var argspec in ArgSpec.Values)
        {
            var t = ArgTypes![count];

            if(t is null) continue;

            var defineResult = scope.Define
            (
                s => new LangtVariable(argspec.Name.ContentStr, t, s)
                {
                    ParameterNumber = count
                }, 
                Range, 
                out var variable
            );

            builder.AddData(defineResult);

            if(variable is not null)
                builder.AddStaticReference(argspec.Name.Range, variable, true);

            count++;
        }
    }
}