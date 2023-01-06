global using CGError = System.ValueTuple<string, Langt.SourceRange>;

using System.Text;
using Langt.Structure;
using Langt.Codegen;
using Langt.Structure.Visitors;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using Langt.Utility;
using Results;

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

    public override void LowerSelf(CodeGenerator generator)
    {
        generator.PushValue( 
            Function!.Type, 
            Function!.LLVMFunction,
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
    public virtual bool IsLValue => false;

    /// <summary>
    /// Whether or not the current AST node has an unknown type, since it has not been 
    /// assigned a valid target type.
    /// </summary>
    public bool HasValidDownflow {get; set;} = false;
    
    /// <summary>
    /// What is the direct type (unaffected by transformers) of this AST node. Should be
    /// equal to <see cref="LangtType.Error"/> if this node is not an expression.
    /// </summary>
    public LangtType RawExpressionType {get; set;} = LangtType.None;
    /// <summary>
    /// What is the inferabble type of this AST node; what type should be used for type
    /// inference of this expression if it is distinct from <see cref="RawExpressionType"/>.
    /// Should be equal to <see langword="null"/> if this node is not an expression, or
    /// there is no distinction between the type to be inferred or the expression type itself. 
    /// </summary>
    public LangtType? NaturalType {get; set;} = null;
    /// <summary>
    /// What is the final, or fully transformed, type of this node.
    /// Found using all the <see cref="ITransformer"/> instances which have been applied to
    /// this AST node.
    /// </summary>
    public LangtType TransformedType => AppliedTransformers.Count > 0 ? AppliedTransformers[^1].Output : RawExpressionType;

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
    public virtual INamedScoped? Resolution {get; init;}

    public LangtType ExpectType => Expect.ValueIs<LangtType>(Resolution, "Expected a type");
    public LangtVariable ExpectVariable => Expect.ValueIs<LangtVariable>(Resolution, "Expected a variable");
    public LangtNamespace ExpectNamespace => Expect.ValueIs<LangtNamespace>(Resolution, "Expected a namespace");

    /// <summary>
    /// The list of transformers currently applied to this AST node, in reverse order 
    /// of their application.
    /// </summary>
    public List<ITransformer> AppliedTransformers {get; init;} = new();

    /// <summary>
    /// Apply the given transform to this AST node.
    /// </summary>
    public void ApplyTransform(ITransformer transformer)
    {
        AppliedTransformers.Add(transformer);
    }

    /// <summary>
    /// Attempt to apply a dereference if this node is an l-value and is a pointer.
    /// </summary>
    /// <seealso cref="IsLValue"/>
    /// <seealso cref="LangtType.IsPointer"/>
    public void TryDeferenceLValue()
    {
        if(!IsLValue) return;
        ApplyTransform(LangtReadPointer.Transformer(TransformedType));
    }

    public Result<BoundASTNode> MatchExprType(ASTPassState state, LangtType type, out bool coerced)
    {
        coerced = false;

        var builder = ResultBuilder.Empty();

        if(HasResolution)
        {
            // Attempt to handle function references
            if(Resolution is LangtFunctionGroup fg)
            {
                if(!type.IsFunctionPtr)
                {
                    return builder
                        .WithDgnError("Cannot target type a function reference to a non-functional type", Range)
                        .Build<BoundASTNode>()
                        .AsTargetTypeDependent();
                }

                var funcType = (LangtFunctionType)type.PointeeType!;

                var fr = fg.ResolveExactOverload(funcType.ParameterTypes, funcType.IsVararg, Range);
                builder.AddData(fr);

                if(!fr) return builder.Build<BoundASTNode>();

                return builder.Build<BoundASTNode>
                (
                    new BoundFunctionReference(this, fr.Value.Function)  
                )
                .AsTargetTypeDependent();
            }
        }

        // Fail if expression has type of 'none'
        if(TransformedType == LangtType.None) return builder
            .WithDgnError($"Expression must have a value", Range)
            .Build<BoundASTNode>();

        // Return here if types match
        if(type == TransformedType) return builder.Build(this);
        
        // Beyond this point, coersion takes place
        coerced = true;

        // Attempt to find a conversion
        var convResult = state.CG.ResolveConversion(type, TransformedType, Range);
        if(!convResult) return builder.WithData(convResult).Build<BoundASTNode>()
                        .AsTargetTypeDependent();

        var conv = convResult.Value;

        // Refuse non-implicit conversions
        if(!conv.IsImplicit)
        {
            return builder
                .WithDgnError($"Could not find conversion from {TransformedType.GetFullName()} to {type.GetFullName()} (an explicit conversion exists)", Range)
                .Build<BoundASTNode>()
                .AsTargetTypeDependent()
            ;
        }

        // Return with conversion
        var res = this with {}; //TODO: explicit copy function should be implemented just in case
        res.ApplyTransform(conv.TransformProvider.TransformerFor(TransformedType, type));
        return builder.Build(res)
            .AsTargetTypeDependent();
    }

    public virtual void LowerSelf(CodeGenerator generator)
    {}
    public void Lower(CodeGenerator generator)
    {   
        generator.Project.Logger.Debug("Lowering " + DebugSourceName, "lowering");

        if(Unreachable)
        {
            return;
        }

        LowerSelf(generator);

        if(AppliedTransformers.Count > 0)
        {
            var v = generator.PopValueNoDebug();
            foreach(var trans in AppliedTransformers)
            {
                generator.Project.Logger.Debug("     Applying transformation " + trans.Name + " . . . ", "lowering");
                v = new(trans.Output, trans.Perform(generator, v.LLVM));
            }
            generator.PushValueNoDebug(v);
        }
        
        generator.LogStack(DebugSourceName);
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

    public Result<BoundASTNode> Bind(ASTPassState state, TypeCheckOptions options = default) 
    {
        var result = BindSelf(state, options);
        if(!result) return result;

        var bast = result.Value;

        // NOTE: does not bind to a variable reference if target type dependent; is this correct?
        if(!result.GetBindingOptions().TargetTypeDependent && bast.HasResolution && bast.Resolution is LangtVariable v) 
        {
            bast = new BoundVariableReference(bast, v)
            {
                RawExpressionType = LangtType.PointerTo(v.Type),
                HasResolution = true,
                Resolution = v
            };

            v.UseCount++;
        }

        if(options.AutoDeferenceLValue) bast.TryDeferenceLValue();

        return result.Map(_ => bast);
    }

    public Result<BoundASTNode> BindMatchingExprType(ASTPassState state, LangtType type, out bool coerced, TypeCheckOptions options = default)
    {
        coerced = false;
        
        //TODO: check that errors are not caused by type checking to report as internal
        //this can be done by adding a 'public record TargetTypeError(Diagnostic Diagnostic) : IResultError'
        //or by making diagnostics passed by along with some record
        //'public record DiagnosticResultError(Diagnostic Diagnostic, bool IsTargetTypeError) : IResultError'

        var binding = Bind(state, options with 
        {
            TargetType = type // TODO: should this always be passed this way?
        });

        if(!binding) return binding;
        var builder = ResultBuilder.From(binding);

        var bast = binding.Value;
        
        var nbast = bast.MatchExprType(state, type, out coerced);
        builder.AddData(nbast);

        if(!nbast) return builder.Build<BoundASTNode>();
        return builder.Build(nbast.Value);
    }
    public Result<BoundASTNode> BindMatchingExprType(ASTPassState state, LangtType type, TypeCheckOptions options = default)
        => BindMatchingExprType(state, type, out _, options);

    // /// <summary>
    // /// Perform the initial steps of type checking.
    // /// Errors in this step should generally be considered fatal or unavoidable.
    // /// The given "target type" from the state may be null.
    // /// </summary>
    // protected virtual void InitialTypeCheckSelf(TypeCheckState state) 
    // {
    //     state.Error("Cannot yet type-check node of type " + GetType().Name, Range);
    // }
    
    // protected virtual void ResetTargetTypeData(TypeCheckState state) 
    // {}
    // /// <summary>
    // /// Perform the final steps of type checking.
    // /// This method, under certain circumstances where multiple type-checks must occur 
    // /// (function overload resolution namely).
    // /// </summary>
    // protected virtual void TargetTypeCheckSelf(TypeCheckState state, LangtType? targetType)
    // {}
    // /// <summary>
    // /// Finalize the type check.
    // /// </summary>
    // protected virtual void FinalTypeCheckSelf(TypeCheckState state)
    // {}

    // private void TypeCheckInternal(TypeCheckState state, LangtType? targetType)
    // {
    //     InitialTypeCheck(state);
    //     TargetTypeCheck(state, targetType);
    //     FinalTypeCheck(state);
    // }

    // public void InitialTypeCheck(TypeCheckState state)
    // {
    //     InitialTypeCheckSelf(state);
    // }
    // public void TargetTypeCheck(TypeCheckState state, LangtType? targetType = null)
    // {
    //     ResetTargetTypeData(state);
    //     TargetTypeCheckSelf(state, targetType);
    // }
    // public void FinalTypeCheck(TypeCheckState state)
    // {
    //     FinalTypeCheckSelf(state);
    //     if(state.TryRead) TryRead();
    //     FinalizedTypeChecking = true;
    // }
    
    // public void TypeCheck(TypeCheckState state, LangtType? targetType = null) 
    // {
    //     state = state with {Noisy=true};

    //     TypeCheckInternal(state, targetType);
    // }
    // 
    // 
    // public bool TryTargetTypeCheck(TypeCheckState state, LangtType? targetType = null)
    // {
    //     try 
    //     {
    //         TargetTypeCheck(state with {Noisy=false}, targetType);
    //     }
    //     catch(TypeCheckException)
    //     {
    //         return false;
    //     }

    //     return true;
    // }
    // public bool TryTypeCheck(TypeCheckState state, LangtType? targetType = null)
    // {
    //     state = state with {Noisy=false};

    //     if(!BlockLike && ContainsInvalid)
    //     {
    //         return true; // TODO: is this valid?
    //     }

    //     return TryPass(s => TypeCheckInternal(s, targetType), state);
    // }

    // public static bool TryPass<T>(Action<T> f, T state) where T: ASTPassState 
    // {
    //     try 
    //     {
    //         f(state);
    //         return true;
    //     }
    //     catch(ASTPassException)
    //     {
    //         return false;
    //     }
    // }

    /*public static void HandleError(ResultException exc, ASTPassState state) //TODO: how will non-errors be handled?
    {
        var (msg, range) = ((string, SourceRange))exc.Error;
        state.CG.Diagnostics.Error(msg, range);
    }*/

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
