using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record DefineStruct(ASTToken Struct, ASTToken Name, ASTToken Open, SeparatedCollection<DefineStructField> Fields, ASTToken Close) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Struct, Name, Open, Fields, Close};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Defining struct " + Name.ContentStr);
        foreach(var f in Fields.Values)
        {
            visitor.Visit(f);
        }
    }

    public LangtStructureType? StructureType {get; private set;}

    public override Result HandleDefinitions(ASTPassState state)
    {
        var t = new LangtStructureType(Name.ContentStr);
        var dt = state.CG.ResolutionScope.DefineType(t, Range);

        if(!dt) return dt;

        StructureType = t;

        return Result.Success();
    }

    public override Result RefineDefinitions(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();

        foreach(var f in Fields.Values) 
        {
            var sfr = f.Field(state);
            builder.AddData(sfr);

            if(!sfr) return builder.Build();

            var sf = sfr.Value;

            if(StructureType!.HasField(sf.Name))
            {
                return ResultBuilder.Empty().WithDgnError($"Cannot redefine field {sf.Name}", Range).Build();
            }

            StructureType.Fields.Add(sf);
        }

        return builder.Build();
    }
}