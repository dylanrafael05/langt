global using CGError = System.ValueTuple<string, Langt.SourceRange>;

using System.Text;
using Langt.Structure;
using Langt.Structure;
using Langt.Structure.Visitors;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using Langt.Utility;
using Results;
using Langt.Structure.Resolutions;

namespace Langt.AST;

public class TreeItemSpec<T>
{
    public string ChildName {get; private init;}
    public IEnumerable<T?> ChildValues {get; private init;}

    public TreeItemSpec(T? node, [CallerArgumentExpression(nameof(node))] string expr = "!")
    {
        ChildName = expr;
        ChildValues = new[] {node};
    }
    public TreeItemSpec(IEnumerable<T?> nodes, [CallerArgumentExpression(nameof(nodes))] string expr = "!")
    {
        ChildName = expr;
        ChildValues = nodes;
    }
}

public class TreeItemContainer<T> : IEnumerable<TreeItemSpec<T>>
{
    private readonly IList<TreeItemSpec<T>> childSpecs = new List<TreeItemSpec<T>>();

    public IEnumerable<TreeItemSpec<T>> ChildrenSpecs => childSpecs;
    public IEnumerable<T?> AllChildren => ChildrenSpecs.SelectMany(c => c.ChildValues);
    public IEnumerable<T> Children => AllChildren.Where(c => c is not null).Cast<T>();

    public void Add(T? node, [CallerArgumentExpression(nameof(node))] string expr = "!")
        => childSpecs.Add(new(node, expr));
    public void Add(IEnumerable<T?> nodes, [CallerArgumentExpression(nameof(nodes))] string expr = "!")
        => childSpecs.Add(new(nodes, expr));
    

    public IEnumerator<TreeItemSpec<T>> GetEnumerator()
    {
        return childSpecs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)childSpecs).GetEnumerator();
    }
}

/// <summary>
/// Represents a tree of arbitrary child count where every item has an associated range.
/// 
/// Children are specified through the 'ChildContainer' property, which also stores information about
/// the expressions they reference for future use.
/// 
/// !: this might be worth removing for performance reasons.
/// 
/// Note that TChild should be be type of the inheriter.
/// </summary>
public abstract record SourcedTreeNode<TChild> : ISourceRanged where TChild : SourcedTreeNode<TChild>, ISourceRanged
{
    public abstract TreeItemContainer<TChild> ChildContainer {get;}

    public IEnumerable<TChild?> AllChildren {get; init;}
    public IEnumerable<TChild> Children {get; init;}


    public virtual SourceRange Range {get; init;}
    public string DebugSourceName {get; init;}


    public SourcedTreeNode()
    {
        var cc = ChildContainer;

        AllChildren = cc.AllChildren.ToArray();
        Children    = cc.Children.ToArray();

        Range = SourceRange.CombineFrom(Children.Select(x => (ISourceRanged)x));
        DebugSourceName = GetType().Name + "@" + Range.CharStart + ":line " + Range.LineStart;
    }

    // TODO: abstract this out to a TreeNode class?
    // Tree operations //
    public void Walk(Predicate<TChild> each)
    {
        if(each((TChild)this))
        {
            foreach(var c in Children) 
            {
                c.Walk(each);
            }
        }
    }

    public bool TryFindFirst(Predicate<TChild> pred, [NotNullWhen(true)] out TChild? item)
    {
        if(pred((TChild)this))
        {
            item = (TChild)this;
            return true;
        }

        foreach(var c in Children)
        {
            if(c.TryFindFirst(pred, out item)) return true;
        }

        item = default;
        return false;
    }

    public bool TryFindLast(Predicate<TChild> pred, [NotNullWhen(true)] out TChild? item)
    {
        if(!pred((TChild)this))
        {
            item = default;
            return false;
        }

        foreach(var c in Children)
        {
            if(c.TryFindLast(pred, out item)) return true;
        }

        item = (TChild)this;
        return true;
    }

    public bool TryFindDeepestContaining(SourceRange range, [NotNullWhen(true)] out TChild? item) 
        => TryFindLast(c => c.Range.Contains(range), out item);
    public bool TryFindDeepestContaining(SourcePosition position, [NotNullWhen(true)] out TChild? item) 
        => TryFindLast(c => c.Range.Contains(position), out item);
}

public record BoundFunctionReference(BoundASTNode Source, LangtFunction Function) : BoundASTNode(Source.ASTSource)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Source};

    public override void LowerSelf(Context generator)
    {
        generator.PushValue( 
            Function!.Type, 
            Function!.LLVMFunction,
            DebugSourceName
        );
    }
}

public record BoundTransform(BoundASTNode Source, ITransformer Transform) : BoundASTNode(Source.ASTSource)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Source};

    public override LangtType Type => Transform.Output;

    public override void LowerSelf(Context generator)
    {
        Source.Lower(generator);
        var pre = generator.PopValue(DebugSourceName);

        generator.PushValue
        (
            Transform.Output, 
            Transform.Perform(generator, pre.LLVM),
            DebugSourceName
        );
    }
}


public abstract record BoundASTNode(ASTNode ASTSource) : SourcedTreeNode<BoundASTNode>, IElement<VisitDumper>
{
    public override SourceRange Range => ASTSource.Range;

    public virtual void Dump(VisitDumper dumper)
    {
        dumper.PutString("bound-node");
        dumper.Visit(ASTSource);
    }

    void IElement<VisitDumper>.OnVisit(VisitDumper visitor)
        => Dump(visitor);
    
    /// <summary>
    /// Is the current AST node representative of an expression which can automatically
    /// be dereferenced or be assigned to if it constitutes a pointer?
    /// TODO: remove in favor of a LangtLValueType or LangtPointerType.IsLValue
    /// </summary>
    [Obsolete("Use LangtReferenceType instead", true)] public virtual bool IsLValue => false;
    
    /// <summary>
    /// What is the direct type of this AST node. Should be
    /// equal to <see cref="LangtType.None"/> if this node is not an expression.
    /// </summary>
    public virtual LangtType Type {get; init;} = LangtType.None;
    /// <summary>
    /// What is the inferabble type of this AST node; what type should be used for type
    /// inference of this expression if it is distinct from <see cref="Type"/>.
    /// Should be equal to <see langword="null"/> if this node is not an expression, or
    /// there is no distinction between the type to be inferred or the expression type itself. 
    /// </summary>
    public LangtType? NaturalType {get; init;} = null;
    /// <summary>
    /// What is the final, or fully transformed, type of this node.
    /// Found using all the <see cref="ITransformer"/> instances which have been applied to
    /// this AST node.
    /// </summary>
    [Obsolete("Use BoundTransform instead", true)] public LangtType TransformedType => AppliedTransformers.Count > 0 ? AppliedTransformers[^1].Output : Type;

    /// <summary>
    /// Whether or not this AST node contains a return statement.
    /// Should always be <see langword="false"/> if this node is not inside a function.
    /// </summary>
    public bool Returns {get; set;} = false;
    /// <summary>
    /// Whether or not this AST node is unreachable due to control flowing out of
    /// a function or due to a degenerate condition.
    /// </summary>
    public bool Unreachable {get; set;} = false;

    public bool IsError {get; set;} = false;
    
    /// <summary>
    /// Whether or not this AST node represents an identifier for a statically resolveable
    /// item.
    /// </summary>
    /// <seealso cref="INamedScoped"/>
    public virtual bool HasResolution {get; init;}
    /// <summary>
    /// The statically resolveable item this AST node represents an identifier for.
    /// Will not be <see langword="null"/> if <see cref="HasResolution"/> returns true.
    /// </summary>
    /// <seealso cref="HasResolution"/>
    /// <seealso cref="INamedScoped"/>
    [MemberNotNullWhen(true, nameof(HasResolution))]
    public virtual IResolution? Resolution {get; init;}

    public LangtType ExpectType => Expect.Is<LangtType>(Resolution, "Expected a type");
    public LangtVariable ExpectVariable => Expect.Is<LangtVariable>(Resolution, "Expected a variable");
    public LangtNamespace ExpectNamespace => Expect.Is<LangtNamespace>(Resolution, "Expected a namespace");

    /// <summary>
    /// The list of transformers currently applied to this AST node, in reverse order 
    /// of their application.
    /// </summary>
    [Obsolete("Use BoundTransform instead", true)] public List<ITransformer> AppliedTransformers {get; init;} = new();

    /// <summary>
    /// Apply the given transform to this AST node.
    /// </summary>
    [Obsolete("Use BoundTransform instead", true)] public void ApplyTransform(ITransformer transformer)
    {
        AppliedTransformers.Add(transformer);
    }

    /// <summary>
    /// Attempt to apply a dereference if this node is an l-value and is a pointer.
    /// </summary>
    /// <seealso cref="IsLValue"/>
    /// <seealso cref="LangtType.IsPointer"/>
    public BoundASTNode TryDeferenceLValue()
    {
        if(!Type.IsReference) return this;
        
        return new BoundTransform(this, DerefenceTransform.For(Type));
    }

    public Result<BoundASTNode> MatchExprType(ASTPassState state, LangtType target, out bool coerced)
    {
        coerced = false;

        var builder = ResultBuilder.Empty();

        if(HasResolution)
        {
            // Attempt to handle function references
            if(Resolution is LangtFunctionGroup fg)
            {
                if(!target.IsFunctionPtr)
                {
                    return builder
                        .WithDgnError("Cannot target type a function reference to a non-functional type", Range)
                        .BuildError<BoundASTNode>()
                        .AsTargetTypeDependent();
                }

                var funcType = (LangtFunctionType)target.ElementType!;

                var fr = fg.ResolveExactOverload(funcType.ParameterTypes, funcType.IsVararg, Range);
                builder.AddData(fr);

                if(!fr) return builder.BuildError<BoundASTNode>();

                return builder.Build<BoundASTNode>
                (
                    new BoundFunctionReference(this, fr.Value.Function)  
                )
                .AsTargetTypeDependent();
            }
        }

        // Fail if expression has type of 'none'
        if(Type == LangtType.None) return builder
            .WithDgnError($"Expression must have a value", Range)
            .BuildError<BoundASTNode>();

        // Return here if types match
        if(target == Type) return builder.Build(this);
        
        // Beyond this point, coersion takes place
        coerced = true;

        // Attempt to find a conversion
        var convResult = state.CG.ResolveConversion(target, Type, Range);
        if(!convResult) return builder.WithData(convResult).BuildError<BoundASTNode>()
                        .AsTargetTypeDependent();

        var conv = convResult.Value;

        // Refuse non-implicit conversions
        if(!conv.IsImplicit)
        {
            return builder
                .WithDgnError($"Could not find conversion from {Type.FullName} to {target.FullName} (an explicit conversion exists)", Range)
                .BuildError<BoundASTNode>()
                .AsTargetTypeDependent()
            ;
        }

        // Return with conversion
        var res = new BoundTransform(this, conv.TransformProvider.TransformerFor(Type, target));
        return builder.Build<BoundASTNode>(res)
            .AsTargetTypeDependent();
    }

    [Obsolete("Use llvm-cg's CodeGenerator.Lower() instead", true)] 
    public virtual void LowerSelf(Context generator)
    {}
    [Obsolete("Use llvm-cg's CodeGenerator.Lower() instead", true)] 
    public void Lower(Context generator)
    {   
        generator.Project.Logger.Debug("Lowering " + DebugSourceName, "lowering");

        if(Unreachable)
        {
            return;
        }

        LowerSelf(generator);
        // generator.LogStack(DebugSourceName);
    }
}

public record BoundASTWrapper(ASTNode Source) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
}

/// <summary>
/// Represents any construct directly present in the AST.
/// </summary>
public abstract record ASTNode : SourcedTreeNode<ASTNode>, IElement<VisitDumper>
{
    /// <summary>
    /// Is the current AST node block-like; should the presence of an invalid child 
    /// prevent type checking from taking place on this node?
    /// </summary>
    public virtual bool BlockLike => false;
    /// <summary>
    /// Is the current AST node untypable; does this AST node require a provided valid
    /// target type in order to type check?
    /// </summary>
    public virtual bool BindingRequiresTargetType => false;

    /// <summary>
    /// Whether or not this AST node is invalid.
    /// </summary>
    /// <remarks>
    /// This is exectly equal to the expression
    /// <code>
    /// node is ASTInvalid
    /// </code>
    /// </remarks>
    public bool IsInvalid => this is ASTInvalid;
    /// <summary>
    /// Whether or not this AST node contains an invalid child node or is itself invalid.
    /// </summary>
    public bool ContainsInvalid => IsInvalid || Children.Any(c => c.ContainsInvalid);


    public virtual Result HandleDefinitions(ASTPassState state) => Result.Success();
    public virtual Result RefineDefinitions(ASTPassState state) => Result.Success();

    protected virtual Result<BoundASTNode> BindSelf(ASTPassState state, TypeCheckOptions options) 
        => Result.Success(new BoundASTWrapper(this) as BoundASTNode);

    public Result<BoundASTNode> Bind(ASTPassState state, TypeCheckOptions? optionsMaybe = null) 
    {
        var options = optionsMaybe ?? new();

        var result = BindSelf(state, options);
        if(!result) return result;

        var bast = result.Value;

        // NOTE: does not bind to a variable reference if target type dependent; is this correct?
        if(!result.GetBindingOptions().TargetTypeDependent && bast.HasResolution && bast.Resolution is LangtVariable v) 
        {
            bast = new BoundVariableReference(bast, v);
            v.UseCount++;
        }

        if(options.AutoDeferenceLValue) 
            bast = bast.TryDeferenceLValue();

        return result.Map(_ => bast);
    }

    public Result<BoundASTNode> BindMatchingExprType(ASTPassState state, LangtType type, out bool coerced, TypeCheckOptions? optionsMaybe = null)
    {
        var options = optionsMaybe ?? new();

        coerced = false;

        var binding = Bind(state, options with 
        {
            TargetType = type // TODO: should this always be passed this way?
        });

        if(!binding) return binding;
        var builder = ResultBuilder.From(binding);

        var bast = binding.Value;
        
        var nbast = bast.MatchExprType(state, type, out coerced);
        builder.AddData(nbast);

        if(!nbast) return builder.BuildError<BoundASTNode>();
        return builder.Build(nbast.Value);
    }
    public Result<BoundASTNode> BindMatchingExprType(ASTPassState state, LangtType type, TypeCheckOptions? optionsMaybe = null)
        => BindMatchingExprType(state, type, out _, optionsMaybe);

    public virtual void Dump(VisitDumper visitor)
    {
        visitor.PutString(GetType().Name);
        foreach(var c in Children) 
        {
            visitor.Visit(c);
        }
    }
    
    void IElement<VisitDumper>.OnVisit(VisitDumper visitor) 
        => Dump(visitor);
}
