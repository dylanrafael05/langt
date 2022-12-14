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

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Variable = generator.ResolutionScope.ResolveVariable(Var.ContentStr, Range, generator.Diagnostics);
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
