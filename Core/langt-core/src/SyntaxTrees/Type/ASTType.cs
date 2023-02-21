using Langt.Structure;
using Langt.Lexing;
using Langt.Structure.Visitors;
using Langt.Utility;
using Langt.Structure.Resolutions;

namespace Langt.AST;

/// <summary>
/// A special case of ASTNode which represents some type information
/// </summary>
public abstract record ASTType() : ASTNode //TODO: implement distinction between type definition and implementation
{
    public abstract Result<Weak<LangtType>> Resolve(ASTPassState state);
}

public record FunctionPtrType(ASTToken Star, ASTToken Fn, ASTToken Open, SeparatedCollection<ASTType> Arguments, ASTToken? Ellipsis, ASTToken Close, ASTType ReturnType) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Star, Fn, Open, Arguments, Ellipsis, Close, ReturnType};

    public override Result<Weak<LangtType>> Resolve(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();

        var retResult = ReturnType.Resolve(state);
        var argsResult = ResultGroup.GreedyForeach
        (
            Arguments.Values,
            t => t.Resolve(state)
        ).Combine();

        builder.WithData(retResult).WithData(argsResult);

        var fType = LangtFunctionType.Create
        (
            parameterTypes: argsResult
                .Or(Arguments.Values.Select(_ => LangtType.Error))!
                .ToArray()!,
            returnType: retResult.Or(LangtType.Error)!, 
            isVararg: Ellipsis is not null
        );

        builder.WithData(fType);

        if(!builder) return builder.BuildError<Weak<LangtType>>();

        return builder.Build<Weak<LangtType>>
        (
            Weak.Wrapping
            (
                LangtPointerType.Create(fType.Value).Expect()
            )
        );
    }
}