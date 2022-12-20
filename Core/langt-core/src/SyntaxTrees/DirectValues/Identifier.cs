using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

// TODO: MAJOR: prevent "cascading" state assignment of pointer reading; reset non-err state variables automatically?

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

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        Resolution = state.CG.ResolutionScope.Resolve(Tok.ContentStr, Range, state);
        HasResolution = Resolution is not null;

        state.CG.Logger.Debug(DebugSourceName + " points to " + Resolution.GetFullName(), "lowering");
        
        if(!IsVariable) return;

        RawExpressionType = LangtType.PointerTo(Variable!.Type);
        Variable.UseCount++;
    }

    protected override void ResetTargetTypeData(TypeCheckState state)
    {
        IsFunction = false;
        Function = null;

        if(IsFunctionGroup) RawExpressionType = LangtType.Error;
    }
    protected override void TargetTypeCheckSelf(TypeCheckState state, LangtType? targetType = null)
    {
        if(!IsFunctionGroup || targetType is null)
        {
            return;
        }
        
        if(!targetType.IsFunctionPtr)
        {
            state.Error("Cannot target-type a reference to a function group to non-function pointer", Range);
        }

        var fp = (LangtFunctionType)targetType.PointeeType!;
        var fg = Resolution as LangtFunctionGroup;

        Function = fg!.ResolveExactOverload(fp.ParameterTypes, fp.IsVararg, this, state, false);

        if(Function is null)
        {
            state.Error("Cannot target-type a reference to a function group to a function pointer which does not match any of the group's overloads", Range);
        }

        RawExpressionType = targetType;
        IsFunction = true;
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(IsVariable)
        {
            lowerer.PushValue( 
                RawExpressionType, Variable!.UnderlyingValue!.LLVM,
                DebugSourceName
            );
        }
        else if(IsFunction)
        {
            lowerer.PushValue( 
                RawExpressionType, Function!.LLVMFunction,
                DebugSourceName
            );
        }
    }
}
