using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record PtrTo(ASTToken PtrToKey, ASTToken Var) : ASTNode, IDirectValue
{
    public override ASTChildContainer ChildContainer => new() {PtrToKey, Var};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Pointer to " + Var.ContentStr);
    }

    public LangtVariable? Variable {get; private set;}

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        Variable = state.CG.ResolutionScope.ResolveVariable(
            Var.ContentStr, 
            Range, 
            state
        );
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        var f = Variable!.UnderlyingValue!;
        lowerer.PushValue( 
            f.Type, 
            f.LLVM,
            DebugSourceName
        );
    }
}
