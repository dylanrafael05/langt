using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineVariable(ASTToken Let, ASTToken Identifier, ASTType? Type, ASTToken Eq, ASTNode Value) : ASTNode
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Let, Identifier, Type, Eq, Value};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Defining variable " + Identifier.Range.Content.ToString());
        visitor.Visit(Value);
    }

    public LangtVariable Variable {get; private set;} = null!;

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        LangtType t;

        if(Type is not null)
        {
            var tn = Type.Resolve(state);
            if(tn is null) return;

            t = tn;
            
            Value.TypeCheck(state, t);
        }
        else
        {
            if(!Value.TryTypeCheck(state))
            {
                state.Error("Cannot use both an inferred type and a target typed expression", Range);
                return;
            }

            t = Value.NaturalType ?? Value.TransformedType;
        }
        
        Variable = new LangtVariable(Identifier.ContentStr, t);
        var couldDef = state.CG.ResolutionScope.DefineVariable(Variable, Range, state);

        if(!couldDef) return;

        if(!state.MakeMatch(t, Value))
        {
            state.Error($"Cannot assign a variable of type {t.Name} to a value of type {Value.TransformedType.Name}", Range);
        }
        
        RawExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(Variable.UseCount > 0)
        {
            Value.Lower(lowerer);
            lowerer.Builder.BuildStore(lowerer.PopValue(DebugSourceName).LLVM, Variable.UnderlyingValue!.LLVM);
        }
        else
        {
            lowerer.Diagnostics.Warning($"Unused variable", Range);
        }
    }
}
