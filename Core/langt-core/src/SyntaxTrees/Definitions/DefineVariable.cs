using Langt.Lexing;
using Langt.Structure;
using Langt.Structure.Visitors;

namespace Langt.AST;

// TODO: some types should only be bound once (like this one
public record BoundVariableDefinition(VariableDefinition Source, LangtVariable Variable, BoundASTNode Value) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value};
}

public record VariableDefinition(ASTToken Let, ASTToken Identifier, ASTType? Type, ASTToken Eq, ASTNode Value) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Let, Identifier, Type, Eq, Value};

    protected override Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options)
    {
        LangtType varT;
        BoundASTNode boundValue;

        var builder = ResultBuilder.Empty();

        if(Type is not null)
        {
            var tn = Type.UnravelSymbol(ctx);
            builder.AddData(tn);
            if(!tn) return builder.BuildError<BoundASTNode>();

            varT = tn.Value;
            
            var bn = Value.BindMatchingExprType(ctx, varT);
            builder.AddData(bn);
            if(!bn) return builder.BuildError<BoundASTNode>();

            boundValue = bn.Value;
        }
        else
        {
            var bn = Value.Bind(ctx);
            builder.AddData(bn);

            if(!bn) return builder.BuildError<BoundASTNode>();

            boundValue = bn.Value;

            varT = boundValue.NaturalType ?? boundValue.Type;
        }

        var variable = new LangtVariable(Identifier.ContentStr, varT, ctx.ResolutionScope) 
        {
            DefinitionRange = Range,
            Documentation = Let.Documentation
        };

        var couldDef = ctx.ResolutionScope.Define(variable, Identifier.Range);
        builder.AddData(couldDef);

        if(!couldDef) return builder.BuildError<BoundASTNode>();
        
        return builder.Build<BoundASTNode>
        (
            new BoundVariableDefinition(this, variable!, boundValue)
            {
                Type = LangtType.None
            }
        );
    }
}
