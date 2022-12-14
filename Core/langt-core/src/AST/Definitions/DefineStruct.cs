using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStruct(ASTToken Struct, ASTToken Name, ASTToken Open, SeparatedCollection<DefineStructField> Fields, ASTToken Close) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Struct, Name, Open, Fields, Close};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Defining struct " + Name.ContentStr);
        foreach(var f in Fields.Values)
        {
            visitor.Visit(f);
        }
    }

    public LangtStructureType? StructureType {get; private set;}

    public override void DefineTypes(CodeGenerator generator)
    {
        var t = new LangtStructureType(Name.ContentStr);
        generator.ResolutionScope.DefineType(t, Range, generator.Diagnostics);

        StructureType = t;
        
        RawExpressionType = LangtType.None;
    }

    public override void ImplementTypes(CodeGenerator generator)
    {
        foreach(var f in Fields.Values) 
        {
            var sf = f.Field(generator);
            if(sf is null) continue;

            if(StructureType!.HasField(sf.Name))
            {
                generator.Diagnostics.Error($"Cannot redefine field {sf.Name}", Range);
            }

            StructureType.Fields.Add(sf);
        }
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {}

    public override void LowerSelf(CodeGenerator generator)
    {}
}