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

    public override void TypeCheckSelf(CodeGenerator generator)
    {
        Left.TypeCheckSelf(generator);
        Right.TypeCheck(generator);

        if(!Left.TransformedType.IsPointer || !Left.IsLValue)
        {
            generator.Diagnostics.Error($"Cannot assign to a non-assignable value", Range);
        }

        if(!generator.MakeMatch(Left.TransformedType.PointeeType!, Right))
        {
            generator.Diagnostics.Error($"Cannot assign value of type {Right.TransformedType.Name} to variable of type {Left.TransformedType.Name}", Range);
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
