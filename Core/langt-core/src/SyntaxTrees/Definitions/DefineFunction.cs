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
                                      LangtScope Scope,
                                      BoundASTNode Body) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Body};

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(Source.Let.Type is TokenType.Extern) return; //work is already done for us when creating the prototypes!
        
        lowerer.BuildFunction(Function, Scope.AllItems.OfType<LangtVariable>(), Body);
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
            .Or(null)
        ;

        if(FunctionGroup is null)
        {
            FunctionGroup = new(Identifier.ContentStr);
            return state.CG.ResolutionScope.DefineFunctionGroup(FunctionGroup, Range); 
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

            ArgTypes[argc++] = a.Value;
        }

        var fnType = new LangtFunctionType(retType, VarargSpec is not null, ArgTypes!);

        var lf = state.CG.CreateNewFunction(Identifier.ContentStr, Let.Type is TokenType.Extern, fnType);
        
        Function = new LangtFunction
        (
            fnType,
            ArgSpec.Values.Select(v => v.Name.ContentStr).ToArray(), 
            lf
        );

        var afr = FunctionGroup!.AddFunctionOverload(Function, Range);
        builder.AddData(afr);

        state.CG.Logger.Debug($"Function overload addition failed: {afr.HasErrors}", "results");

        return builder.Build();
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        if(Let.Type is TokenType.Extern || Function is null)
        {
            // TODO: prechcek extern existence
            return Result.Success<BoundASTNode>(new BoundASTWrapper(this));
        }

        var previousFunction = state.CG.CurrentFunction;

        state.CG.CurrentFunction = Function;
        var scope = state.CG.CreateUnnamedScope();

        var count = 0u;
        foreach(var argspec in ArgSpec.Values)
        {
            var t = ArgTypes![count];

            if(t is null) continue;

            builder.AddData
            (
                scope.DefineVariable
                (
                    new(argspec.Name.ContentStr, t)
                    {
                        ParameterNumber = count
                    }, 
                    Range
                )
                .Forgive()
            );

            count++;
        }

        Result<BoundASTNode> br;

        if(Function.Type.ReturnType == LangtType.None)
        {
            br = Body!.Bind(state);
        }
        else 
        {
            br = Body!.BindMatching(state, Function.Type.ReturnType);
        }

        builder.AddData(br);
        if(!br) return builder.Build<BoundASTNode>();

        state.CG.CloseScope();
        state.CG.CurrentFunction = previousFunction;

        return builder.Build<BoundASTNode>
        (
            new BoundFunctionDefinition(this, Function, scope, br.Value)  
        );
    }
}