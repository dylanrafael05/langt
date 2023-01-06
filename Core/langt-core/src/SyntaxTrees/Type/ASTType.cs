using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;
using Langt.Utility;

namespace Langt.AST;

/// <summary>
/// A special case of ASTNode which represents some type information
/// </summary>
public abstract record ASTType() : ASTNode //TODO: implement distinction between type definition and implementation
{
    public abstract Result<LangtType> Resolve(ASTPassState state);
}

public record FunctionPtrType(ASTToken Star, ASTToken Open, SeparatedCollection<ASTType> Arguments, ASTToken? Ellipsis, ASTToken Close, ASTType ReturnType) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Star, Open, Arguments, Ellipsis, Close, ReturnType};

    public override Result<LangtType> Resolve(ASTPassState state)
    {
        var retResult = ReturnType.Bind(state);
        var argsResult = ResultGroup.GreedyForeach
        (
            Arguments.Values,
            t => t.Bind(state)
        ).Combine();

        var fType = new LangtFunctionType
        (
            retResult.Map(k => k.Resolution as LangtType).Or(LangtType.Error)!, 
            Ellipsis is not null, 
            argsResult
                .Map(k => k.Select(x => x.Resolution as LangtType))
                .Or(Arguments.Values.Select(_ => LangtType.Error))!
                .ToArray()!
        );

        var builder = ResultBuilder.From(retResult, argsResult);

        if(!builder) return builder.Build<LangtType>();

        return builder.Build<LangtType>
        (
            new LangtPointerType(fType)
        );
    }
}