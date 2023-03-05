using Langt.Lexing;
using Langt.Message;
using Langt.Structure;

using Langt.Structure.Visitors;

namespace Langt.AST;

public record GenericParameterSpecification(ASTToken Bang, ASTToken Start, SeparatedCollection<ASTToken> TypeSpecs, ASTToken End) : ASTNode 
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Bang, Start, TypeSpecs, End};
}
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

    private LangtFunctionGroup? FunctionGroup {get; set;}
    private LangtFunction? Function {get; set;}
    private LangtType?[]? ArgTypes {get; set;}

    public override Result HandleDefinitions(Context ctx)
    {
        FunctionGroup = ctx.ResolutionScope.ResolveDirect
            (
                Identifier.ContentStr, 
                Range,
                ctx,
                false
            )
            .AsOr<LangtFunctionGroup>(null)
            .OrDefault()
        ;

        if(FunctionGroup is null)
        {
            FunctionGroup = new LangtFunctionGroup(Identifier.ContentStr, ctx.ResolutionScope);
            return ctx.ResolutionScope.Define(FunctionGroup, Range); 
        }
        else
        {
            return Result.Success();
        }
    }

    public override Result RefineDefinitions(Context ctx)
    {
        var builder = ResultBuilder.Empty();

        if(Let.Type is not TokenType.Extern && VarargSpec is not null)
        {
            return builder
                .WithDgnError(Messages.Get("fn-not-extern-vararg"), Range)
                .Build();
        }

        var fsym = new FunctionTypeSymbol
        {
            ReturnType     = Type.GetSymbol(ctx),
            ParameterTypes = ArgSpec.Values.Select(a => a.Type.GetSymbol(ctx)).ToArray(), 
            IsVararg       = VarargSpec is not null,
            ParameterNames = ArgSpec.Values.Select(k => k.Name.ContentStr).ToArray()
        };

        var fnres = fsym.Unravel(ctx);
        builder.AddData(fnres);

        if(!builder) return builder.Build();

        if (fnres.Value is not LangtFunctionType fnType)
            throw new UnreachableException();

        Function = new LangtFunction(FunctionGroup!)
        {
            Type            = fnType,
            ParameterNames  = ArgSpec.Values.Select(v => v.Name.ContentStr).ToArray(),
            
            IsExtern        = Let.Type is TokenType.Extern,

            Documentation   = Let.Documentation,
            DefinitionRange = SourceRange.CombineFrom(Let, Type)
        };

        var afr = FunctionGroup!.AddFunctionOverload(Function, Range);
        builder.AddData(afr);

        builder.AddStaticReference(Identifier.Range, Function, true);

        return builder.Build();
    }

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        var builder = ResultBuilder.Empty();

        if(Let.Type is TokenType.Extern || Function is null)
        {
            // TODO: precheck extern existence
            return Result.Success<BoundASTNode>(new BoundEmpty(this));
        }

        var previousFunction = ctx.CurrentFunction;
        
        Result<BoundASTNode> bodyResult;

        ctx.CurrentFunction = Function;
        var scope = ctx.OpenScope();
        {
            DefineParametersInScope(builder, scope);

            if(Function.Type.ReturnType == LangtType.None || Body is FunctionBlockBody)
            {
                bodyResult = Body!.Bind(ctx, new TypeCheckOptions {PredefinedBlockScope = scope});
            }
            else 
            {
                bodyResult = Body!.Bind(ctx, new TypeCheckOptions {PredefinedBlockScope = scope, TargetType = Function.Type.ReturnType});
            }

            if(!builder.WithData(bodyResult)) return builder.BuildError<BoundASTNode>();
        }
        ctx.CloseScope();
        ctx.CurrentFunction = previousFunction;

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

            var variable = new LangtVariable(argspec.Name.ContentStr, t, scope)
            {
                ParameterNumber = count
            };

            var defineResult = scope.Define
            (
                new LangtVariable(argspec.Name.ContentStr, t, scope)
                {
                    ParameterNumber = count
                }, 
                Range
            );

            builder.AddData(defineResult);

            if(defineResult)
                builder.AddStaticReference(argspec.Name.Range, variable, true);

            count++;
        }
    }
}