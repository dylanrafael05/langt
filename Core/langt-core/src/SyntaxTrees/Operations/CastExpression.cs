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

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Cast");
        visitor.Visit(Value);
        visitor.Visit(Type);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var results = Result.All
        (
            Value.Bind(state),
            Type.Resolve(state)
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
        var cv = state.CTX.ResolveConversion(type, val.Type, Range);
        builder.AddData(cv);

        if(!cv) return builder.BuildError<BoundASTNode>();
        var conversion = cv.Value;

        // TODO: make RawExpressionType required or a parameter
        return builder.Build<BoundASTNode>
        (
            new BoundConversion(val with {ASTSource = this}, conversion)
            {
                Type = type
            }
        );
    }
}