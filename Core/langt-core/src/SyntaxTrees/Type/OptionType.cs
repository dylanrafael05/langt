using Langt.Structure;

namespace Langt.AST;

public record OptionType(ASTType Left, ASTToken Pipe, ASTType Right) : ASTType
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Left, Pipe, Right};

    public override Result<LangtType> Resolve(ASTPassState state)
    {
        var builder = ResultBuilder.Empty();

        var l = Left.Resolve(state);
        builder.AddData(l);
        if(!l) return builder.BuildError<LangtType>();

        var r = Right.Resolve(state);
        builder.AddData(r);
        if(!r) return builder.BuildError<LangtType>();

        Result<LangtType> Dup() 
            => builder!.WithDgnError($"Duplicate option {l.Value.FullName}", Left.Range).BuildError<LangtType>();

        IEnumerable<LangtType> resOpts;

        if(r.Value is LangtOptionType opt) 
        {
            if(opt.OptionTypes.Contains(l.Value)) return Dup();

            resOpts = opt.OptionTypes.Append(l.Value);
        }
        else 
        {
            if(l.Value == r.Value) return Dup();

            resOpts = new[] {l.Value, r.Value};
        }

        var k = LangtOptionType.Create(resOpts.ToHashSet(), Range);
        builder.AddData(k);
        return builder.Build<LangtType>(k.PossibleValue!);
    }
}