using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundPointerCastExpression(CastExpression Source, BoundASTNode Value, LangtPointerType CastType) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value};

    public override LangtType Type => CastType;
}

public record BoundAliasCastExpression(CastExpression Source, BoundASTNode Value, LangtType AliasType) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value};

    public override LangtType Type => AliasType;
}

public record CastExpression(ASTNode Value, ASTToken As, ASTType Type) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Value, As, Type};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        var results = Result.All
        (
            Value.Bind(ctx),
            Type.UnravelSymbol(ctx)
        );
        var builder = ResultBuilder.From(results);

        if(!results) return builder.BuildError<BoundASTNode>();

        var (val, type) = results.Value;

        // Handle pointer bitcasting
        if(val.Type.IsPointer && type.IsPointer)
        {
            return builder.Build<BoundASTNode>
            (
                new BoundPointerCastExpression(this, val, type.Pointer)
            );
        }

        // Handle alias type casting
        if((val.Type.IsAlias && type == val.Type.ElementType) || (type.IsAlias && val.Type == type.ElementType))
        {
            return builder.Build<BoundASTNode>
            (
                new BoundAliasCastExpression(this, val, type)
            );
        }

        // Conversion
        var cv = ctx.ResolveConversion(val.Type, type, Range);
        builder.AddData(cv);

        if(!cv) return builder.BuildError<BoundASTNode>();
        var conversion = cv.Value;

        // TODO: make RawExpressionType required or a parameter
        return builder.Build<BoundASTNode>
        (
            new BoundConversion(val, conversion)
            {
                Type = type
            }
        );
    }
}