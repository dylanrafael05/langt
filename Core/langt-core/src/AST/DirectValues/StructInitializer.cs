using Langt.Lexing;
using Langt.Codegen;
using Langt.Structure.Visitors;

namespace Langt.AST;

public record StructInitializer(ASTType Type, ASTToken Open, SeparatedCollection<ASTNode> Args, ASTToken Close) : ASTNode
{
    public override ASTChildContainer ChildContainer => new() {Type, Open, Args, Close};

    public override void Dump(VisitDumper visitor)
    {
        visitor.PutString("Creating struct");
        visitor.Visit(Type);
        foreach(var a in Args.Values)
        {
            visitor.Visit(a);
        }
    }

    public override void TypeCheckRaw(CodeGenerator generator)
    {
        var t = Type.Resolve(generator);
        if(t is null) return;
        
        ExpressionType = t;

        if(!ExpressionType.IsStructure)
        {
            generator.Diagnostics.Error($"Unknown structure type {ExpressionType.Name}", Range);
            return;
        }

        var args = Args.Values.ToList();

        if(ExpressionType.Structure!.Fields.Count != args.Count)
        {
            generator.Diagnostics.Error($"Incorrect number of fields for structure initializer of type {ExpressionType.Name}", Range);
            return;
        }

        for(int i = 0; i < args.Count; i++)
        {
            var f = ExpressionType.Structure!.Fields[i];
            var ftype = f.Type;
            var fname = f.Name;

            args[i].TypeCheck(generator);

            if(!generator.MakeMatch(ftype, args[i]))
            {
                generator.Diagnostics.Error($"Incorrect type for field '{fname}' in struct initializer for struct {ExpressionType.Name}; expected {ftype.Name} but got {args[i].TransformedType.Name}", Range);
            }
        }
    }

    public override void LowerSelf(CodeGenerator lowerer)
    {
        var llvmArgs = new List<LLVMValueRef>();
        var args = Args.Values.ToList();

        foreach(var arg in args)
        {
            arg.Lower(lowerer);
            llvmArgs.Add(lowerer.PopValue().LLVM);
        }

        var structBuild = lowerer.Builder.BuildAlloca(lowerer.LowerType(ExpressionType), ExpressionType.Name+".builder");

        for(int i = 0; i < llvmArgs.Count; i++)
        {
            var fptr = lowerer.Builder.BuildStructGEP2(lowerer.LowerType(ExpressionType), structBuild, (uint)i, ExpressionType.Name+".builder.element."+i);
            lowerer.Builder.BuildStore(llvmArgs[i], fptr);
        }

        lowerer.PushValue(
            ExpressionType,
            lowerer.Builder.BuildLoad2(lowerer.LowerType(ExpressionType), structBuild, ExpressionType.Name)
        );
    }
}