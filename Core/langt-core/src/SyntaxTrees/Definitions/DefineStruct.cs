using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStruct(ASTToken Struct, ASTToken Name, ASTToken Open, SeparatedCollection<DefineStructField> Fields, ASTToken Close) : ASTNode
{
    public override RecordItemContainer<ASTNode> ChildContainer => new() {Struct, Name, Open, Fields, Close};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Defining struct " + Name.ContentStr);
        foreach(var f in Fields.Values)
        {
            visitor.Visit(f);
        }
    }

    public LangtStructureType? StructureType {get; private set;}

    public override void DefineTypes(ASTPassState state)
    {
        var t = new LangtStructureType(Name.ContentStr);
        state.CG.ResolutionScope.DefineType(t, Range, state);

        StructureType = t;
        
        RawExpressionType = LangtType.None;
    }

    public override void ImplementTypes(ASTPassState state)
    {
        foreach(var f in Fields.Values) 
        {
            var sf = f.Field(state);
            if(sf is null) continue;

            if(StructureType!.HasField(sf.Name))
            {
                state.Error($"Cannot redefine field {sf.Name}", Range);
            }

            StructureType.Fields.Add(sf);
        }
    }

    protected override void InitialTypeCheckSelf(TypeCheckState state)
    {}

    public override void LowerSelf(CodeGenerator generator)
    {}
}