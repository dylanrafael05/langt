using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record Assignment(ASTNode Left, ASTToken Assign, ASTNode Right) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Left, Assign, Right};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString($"Assignment");
        visitor.Visit(Left);
        visitor.Visit(Right);
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {
        Left.TypeCheck(state with {TryRead = false});
        Right.TypeCheck(state);

        if(!Left.TransformedType.IsPointer || !Left.IsLValue)
        {
            state.Error($"Cannot assign to a non-assignable value", Range);
        }

        if(!state.MakeMatch(Left.TransformedType.PointeeType!, Right))
        {
            state.Error($"Cannot assign value of type {Right.TransformedType.Name} to variable of type {Left.TransformedType.Name}", Range);
        }
        
        RawExpressionType = LangtType.None;

        // TODO: make reading pointers the default, have a special case (stored in codeGenerator), for when values should be UNREAD
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        Left.Lower(lowerer);
        Right.Lower(lowerer);

        var (right, left) = (lowerer.PopValue(DebugSourceName), lowerer.PopValue(DebugSourceName));

        lowerer.Builder.BuildStore(right.LLVM, left.LLVM);
    }
}
