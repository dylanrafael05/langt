using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

// TODO: some types should only be bound once (like this one
public record BoundVariableDefinition(VariableDefinition Source, LangtVariable Variable, BoundASTNode Value) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Value};

    public override void LowerSelf(CodeGenerator generator)
    {
        if(Variable.UseCount > 0)
        {
            Value.Lower(generator);
            generator.Builder.BuildStore(generator.PopValue(DebugSourceName).LLVM, Variable.UnderlyingValue!.LLVM);
        }
    }
}

public record VariableDefinition(ASTToken Let, ASTToken Identifier, ASTType? Type, ASTToken Eq, ASTNode Value) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Let, Identifier, Type, Eq, Value};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Defining variable " + Identifier.Range.Content.ToString());
        visitor.Visit(Value);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        LangtType varT;
        BoundASTNode boundValue;

        var builder = ResultBuilder.Empty();

        if(Type is not null)
        {
            var tn = Type.Resolve(state);
            builder.AddData(tn);
            if(!tn) return builder.Build<BoundASTNode>();

            varT = tn.Value;
            
            var bn = Value.BindMatchingExprType(state, varT);
            builder.AddData(bn);
            if(!bn) return builder.Build<BoundASTNode>();

            boundValue = bn.Value;
        }
        else
        {
            var bn = Value.Bind(state);
            builder.AddData(bn);

            if(!bn) return builder.Build<BoundASTNode>();

            boundValue = bn.Value;

            varT = boundValue.NaturalType ?? boundValue.TransformedType;
        }
        
        var variable = new LangtVariable(Identifier.ContentStr, varT);

        var couldDef = state.CG.ResolutionScope.DefineVariable(variable, Range);
        builder.AddData(couldDef);

        if(!couldDef) return builder.Build<BoundASTNode>();
        
        return builder.Build<BoundASTNode>
        (
            new BoundVariableDefinition(this, variable, boundValue)
            {
                RawExpressionType = LangtType.None
            }
        );
    }
}
