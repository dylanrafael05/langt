using Langt.Codegen;
using Langt.Lexing;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DotAccess(ASTNode Left, ASTToken Dot, ASTToken Right) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Left, Dot, Right};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString(". access");
        visitor.Visit(Left);
        visitor.Visit(Right);
    }

    public LangtStructureField? Field {get; private set;}
    public int? FieldIndex {get; private set;}

    public override bool Readable => true;


    public override void TypeCheckRaw(CodeGenerator generator)
    {
        Left.TypeCheckRaw(generator);

        HasResolution = Left.HasResolution && Left.Resolution is not LangtVariable;

        if(HasResolution)
        {
            if(Left.Resolution is not LangtNamespace ns) 
            {
                generator.Diagnostics.Error($"Cannot access a static member of something that is not a namespace", Range);
                return;
            }

            Resolution = ns.Resolve(Right.ContentStr, Range, generator.Diagnostics);

            return;
        }

        if(!Left.TransformedType.IsPointer || Left.TransformedType.PointeeType is not LangtStructureType structureType)
        {
            generator.Diagnostics.Error($"Cannot use a '.' access on a non-structure type", Range);
            return;

            // TODO: modify this to include namespace getters! (how will that work?)
        }

        if(!structureType.TryResolveField(Right.ContentStr, out var field, out var index, out _))
        {
            generator.Diagnostics.Error($"Unknown field {Right.ContentStr} for type {structureType.Name}", Range);
            return;
        }

        Field = field;
        FieldIndex = index;

        ExpressionType = LangtType.PointerTo(Field!.Type);
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        if(HasResolution) return;

        Left.Lower(lowerer);

        var s = lowerer.PopValue();

        lowerer.PushValue(
            ExpressionType,
            lowerer.Builder.BuildStructGEP2(
                lowerer.LowerType(s.Type.PointeeType!), 
                s.LLVM,
                (uint)FieldIndex!.Value,
                s.Type.PointeeType!.Name + "." + Field!.Name
            )
        );
    }
}
