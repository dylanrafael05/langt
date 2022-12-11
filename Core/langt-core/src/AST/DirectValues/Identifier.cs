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

    private LangtFunction? GetOverloadFromDownflow(LangtType downflowType, CodeGenerator generator, bool err)
    {
        var fp = (LangtFunctionType)downflowType.PointeeType!;
        var fg = Resolution as LangtFunctionGroup;

        return fg!.ResolveOverload(fp, Range, generator, err: err);
    }

    public override bool IsValidDownflow(LangtType? type, CodeGenerator generator)
    {
        if(!IsFunctionGroup)
        {
            return true;
        }

        if(type is null || !type.IsFunctionPtr)
        {
            return false;
        }
        
        var overl = GetOverloadFromDownflow(type, generator, false);

        return overl is not null;
    }
    public override void AcceptDownflow(LangtType? type, CodeGenerator generator)
    {
        if(!IsFunctionGroup)
        {
            return;
        }

        if(type is null || !type.IsFunctionPtr)
        {
            throw new Exception("This should not be the case!");
        }

        Function = GetOverloadFromDownflow(type, generator, true);
        ExpressionType = type;
        IsFunction = true;

        generator.Logger.Note("Downflow for identifier accepted: " + Function);
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
            var f = Function!.LLVMFunction;
            lowerer.PushValue(
                ExpressionType, Function!.LLVMFunction
            );
        }
    }
}
