using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundPtrTo(PtrTo Source, LangtVariable Variable) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};

    public override void LowerSelf(CodeGenerator generator)
    {
        var f = Variable!.UnderlyingValue!;
        generator.PushValue( 
            f.Type, 
            f.LLVM,
            DebugSourceName
        );
    }
}

public record PtrTo(ASTToken PtrToKey, ASTToken Var) : ASTNode, IDirectValue
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {PtrToKey, Var};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Pointer to " + Var.ContentStr);
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var vResult = state.CG.ResolutionScope.ResolveVariable
        (
            Var.ContentStr, 
            Range
        );

        if(!vResult) return vResult.ErrorCast<BoundASTNode>();

        return Result.Success<BoundASTNode>(new BoundPtrTo(this, vResult.Value)
        {
            Type = LangtPointerType.Create(vResult.Value.Type.ElementType!)
                .Expect("Errors should not occur while getting a pointer to a variable")
        });
    }
}
