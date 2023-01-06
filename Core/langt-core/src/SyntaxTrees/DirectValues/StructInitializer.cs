using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record BoundStructInitializer(StructInitializer Source, BoundASTNode[] BoundArgs) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {BoundArgs};

    public override void LowerSelf(CodeGenerator lowerer)
    {
        var llvmArgs = new List<LLVMValueRef>();

        foreach(var arg in BoundArgs)
        {
            arg.Lower(lowerer);
            llvmArgs.Add(lowerer.PopValue(DebugSourceName).LLVM);
        }

        var structBuild = lowerer.Builder.BuildAlloca(lowerer.LowerType(RawExpressionType), RawExpressionType.Name+".builder");

        for(int i = 0; i < llvmArgs.Count; i++)
        {
            var fptr = lowerer.Builder.BuildStructGEP2(lowerer.LowerType(RawExpressionType), structBuild, (uint)i, RawExpressionType.Name+".builder.element."+i);
            lowerer.Builder.BuildStore(llvmArgs[i], fptr);
        }

        lowerer.PushValue( 
            RawExpressionType,
            lowerer.Builder.BuildLoad2(lowerer.LowerType(RawExpressionType), structBuild, RawExpressionType.Name),
            DebugSourceName
        );
    }
}

public record StructInitializer(ASTType Type, ASTToken Open, SeparatedCollection<ASTNode> Args, ASTToken Close) : ASTNode
{
    public override TreeItemContainer<ASTNode> ChildContainer => new() {Type, Open, Args, Close};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Creating struct");
        visitor.Visit(Type);
        foreach(var a in Args.Values)
        {
            visitor.Visit(a);
        }
    }

    protected override Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options)
    {
        var tn = Type.Resolve(state);
        var builder = ResultBuilder.From(tn);
        if(!tn) return tn.Cast<BoundASTNode>();

        var type = tn.Value;

        if(!type.IsStructure)
        {
            return builder.WithDgnError($"Unknown structure type {type.Name}", Range).Build<BoundASTNode>();
        }

        var args = Args.Values.ToList();

        if(type.Structure!.Fields.Count != args.Count)
        {
            return builder.WithDgnError($"Incorrect number of fields for structure initializer of type {type.Name}", Range)
                .Build<BoundASTNode>();
        }

        var results = ResultGroup.GreedyForeach
        (
            args.Indexed(),
            a => 
            {
                var f = type.Structure!.Fields[a.Index];
                
                var ftype = f.Type;
                var fname = f.Name;

                var r = a.Value.BindMatchingExprType(state, ftype);

                if(!r) return Result.Error<BoundASTNode>
                (
                    Diagnostic.Error($"Incorrect type for field '{fname}' in struct initializer for struct {type.Name}", Range)
                );

                return r;
            }
        );
        builder.AddData(results);

        if(!results) return builder.Build<BoundASTNode>();

        return builder.Build<BoundASTNode>
        (
            new BoundStructInitializer(this, results.Value.ToArray())
            {
                RawExpressionType = type
            }
        );

        // TODO: add another syntax tree? resolved syntax tree?
    }
}