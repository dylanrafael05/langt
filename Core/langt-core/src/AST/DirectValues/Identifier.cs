using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

public record Identifier(ASTToken Tok) : ASTNode, IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {Tok};

    public override void Dump(VisitDumper visitor)
        => visitor.VisitNoDepth(Tok);

    public override bool IsLValue => IsVariable;
    public override bool RequiresTypeDownflow => IsFunctionGroup;

    public bool IsVariable => Resolution is (not null) and LangtVariable;
    public bool IsFunctionGroup => Resolution is (not null) and LangtFunctionGroup;
    public bool IsFunction {get; private set;}

    public LangtVariable? Variable => Resolution as LangtVariable;
    public LangtFunction? Function {get; private set;}

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        HasResolution = true;
        Resolution = generator.ResolutionScope.Resolve(Tok.ContentStr, Range, generator.Diagnostics);

        //generator.Logger.Note("IDENTIFIER POITNS TO " + Resolution);
        
        if(!IsVariable) return;

        ExpressionType = LangtType.PointerTo(Variable!.Type);
        Variable.UseCount++;

        generator.Logger.Note("Identifier is variable: " + Variable.GetFullName());
    }

    public override bool AcceptDownflow(LangtType? type, CodeGenerator generator, bool err = true)
    {
        if(!IsFunctionGroup)
        {
            return true;
        }

        if(type is null || !type.IsFunctionPtr)
        {
            if(err) generator.Diagnostics.Error("Cannot target-type a reference to a function group to non-function pointer", Range);
            return false;
        }

        var fp = (LangtFunctionType)type.PointeeType!;
        var fg = Resolution as LangtFunctionGroup;

        Function = fg!.ResolveOverload(fp, Range, generator, false);

        if(Function is null)
        {
            if(err) generator.Diagnostics.Error("Cannot target-type a reference to a function group to a function pointer which does not match any of the group's overloads", Range);
            return false;
        }

        ExpressionType = type;
        IsFunction = true;

        return true;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(IsVariable)
        {
            lowerer.PushValue(
                ExpressionType, Variable!.UnderlyingValue!.LLVM
            );
        }
        else if(IsFunction)
        {
            lowerer.PushValue(
                ExpressionType, Function!.LLVMFunction
            );
        }
    }
}
