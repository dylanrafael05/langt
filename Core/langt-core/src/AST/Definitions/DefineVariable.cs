using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineVariable(ASTToken Let, ASTToken Identifier, ASTType? Type, ASTToken Eq, ASTNode Value) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Let, Identifier, Type, Eq, Value};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Defining variable " + Identifier.Range.Content.ToString());
        visitor.Visit(Value);
    }

    public LangtVariable Variable {get; private set;} = null!;

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Value.TypeCheck(generator);

        LangtType t;

        if(Type is not null)
        {
            var tn = Type.Resolve(generator);
            if(tn is null) return;

            t = tn;
        }
        else
        {
            t = Value.InferrableType ?? Value.TransformedType;
        }
        
        Variable = new LangtVariable(Identifier.ContentStr, t);
        var couldDef = generator.ResolutionScope.DefineVariable(Variable, Range, generator.Project.Diagnostics);

        if(!couldDef) return;

        if(!generator.MakeMatch(t, Value))
        {
            generator.Diagnostics.Error($"Cannot assign a variable of type {t.Name} to a value of type {Value.TransformedType.Name}", Range);
        }
        
        ExpressionType = LangtType.None;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(Variable.UseCount > 0)
        {
            Value.Lower(lowerer);
            lowerer.Builder.BuildStore(lowerer.PopValue().LLVM, Variable.UnderlyingValue!.LLVM);
        }
        else
        {
            lowerer.Diagnostics.Warning($"Unused variable", Range);
        }
    }
}
