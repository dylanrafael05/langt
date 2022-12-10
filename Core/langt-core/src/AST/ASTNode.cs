using System.Text;
using Langt.Structure;
using Langt.Codegen;
using Langt.Structure.Visitors;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Langt.AST;

public class ASTChildSpec
{
    public string ChildName {get; private init;}
    public IEnumerable<ASTNode?> ChildValues {get; private init;}

    public ASTChildSpec(ASTNode? node, [CallerArgumentExpression(nameof(node))] string expr = "!")
    {
        ChildName = expr;
        ChildValues = new[] {node};
    }
    public ASTChildSpec(IEnumerable<ASTNode?> nodes, [CallerArgumentExpression(nameof(nodes))] string expr = "!")
    {
        ChildName = expr;
        ChildValues = nodes;
    }
}

public class ASTChildContainer : IEnumerable<ASTChildSpec>
{
    private readonly IList<ASTChildSpec> childSpecs = new List<ASTChildSpec>();

    public IEnumerable<ASTChildSpec> ChildrenSpecs => childSpecs;
    public IEnumerable<ASTNode?> AllChildren => ChildrenSpecs.SelectMany(c => c.ChildValues);
    public IEnumerable<ASTNode> Children => AllChildren.Where(c => c is not null).Cast<ASTNode>();

    public void Add(ASTNode? node, [CallerArgumentExpression(nameof(node))] string expr = "!")
        => childSpecs.Add(new(node, expr));
    public void Add(IEnumerable<ASTNode> nodes, [CallerArgumentExpression(nameof(nodes))] string expr = "!")
        => childSpecs.Add(new(nodes, expr));
    

    public IEnumerator<ASTChildSpec> GetEnumerator()
    {
        return childSpecs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)childSpecs).GetEnumerator();
    }
}

/// <summary>
/// Represents any construct directly present in the AST.
/// </summary>
public abstract record ASTNode : ISourceRanged, IElement<VisitDumper>
{
    public abstract ASTChildContainer ChildContainer {get;}

    public IEnumerable<ASTNode?> AllChildren => ChildContainer.AllChildren;
    public IEnumerable<ASTNode> Children => ChildContainer.Children;

    public virtual SourceRange Range => SourceRange.CombineFrom(Children);

    private static IEnumerable<ASTNode> ChildList(params object?[] inputs)
    {
        foreach(var i in inputs)
        {
            if(i is ASTNode a) yield return a;
            else if(i is IEnumerable ie) foreach(var o in ChildList(ie.Cast<object>().ToArray())) yield return o;
            else if(i is null) {}
            else throw new Exception("Cannot unwrap non-ASTNode items");
        }
    }

    public virtual void Dump(VisitDumper visitor)
    {
        visitor.PutString(GetType().Name);
        foreach(var c in Children) 
        {
            visitor.Visit(c);
        }
    }
    
    public LangtType ExpressionType {get; set;} = LangtType.Error;
    public LangtType? InferrableType {get; set;} = null;
    public LangtType TransformedType => AppliedTransformers.Count > 0 ? AppliedTransformers[^1].Output : ExpressionType;

    public bool Returns {get; set;} = false;
    public bool Unreachable {get; set;} = false;
    public virtual bool Readable => false;

    public bool IsInvalid => this is ASTInvalid;
    public bool ContainsInvalid => IsInvalid || Children.Any(c => c.ContainsInvalid);
    
    public bool HasResolution {get; set;}
    public INamedScoped? Resolution {get; set;}

    public List<ITransformer> AppliedTransformers {get; private init;} = new();

    public void ApplyTransform(ITransformer transformer)
    {
        AppliedTransformers.Add(transformer);
    }
    public void TryRead()
    {
        if(!Readable || !TransformedType.IsPointer) return;
        ApplyTransform(LangtReadPointer.Transformer(TransformedType));
    }

    // TODO: Reimplement to allow for 'before all' / 'after all' hooks

    public virtual void Initialize(CodeGenerator generator)
    {}
    public virtual void DefineTypes(CodeGenerator generator)
    {}
    public virtual void ImplementTypes(CodeGenerator generator)
    {}
    public virtual void DefineFunctions(CodeGenerator generator)
    {}
    public virtual void TypeCheckRaw(CodeGenerator generator)
        => throw new NotImplementedException("Cannot yet type-check AST Node of type " + GetType().Name);
    public void TypeCheck(CodeGenerator generator)
    {
        if(BlockLike || !ContainsInvalid)
        {
            TypeCheckRaw(generator);
            TryRead();
        }
    }

    public virtual bool BlockLike => Children.Any(c => c.BlockLike);

    public virtual void LowerSelf(CodeGenerator generator)
        => throw new NotImplementedException("Cannot yet lower AST Node of type " + GetType().Name);
    
    public void Lower(CodeGenerator generator)
    {
        generator.Project.Logger.Note("Lowering " + GetType().Name);

        if(Unreachable)
        {
            generator.Project.Diagnostics.Warning("Unreachable code detected", Range);
            return;
        }

        LowerSelf(generator);

        if(AppliedTransformers.Count > 0)
        {
            var v = generator.PopValue();
            foreach(var trans in AppliedTransformers)
            {
                generator.Project.Logger.Note("    Applying transformation " + trans.Name + " . . . ");
                v = new(trans.Output, trans.Perform(generator, v.LLVM));
            }
            generator.PushValue(v);
        }
    }
    
    void IElement<VisitDumper>.OnVisit(VisitDumper visitor) 
        => Dump(visitor);
}
