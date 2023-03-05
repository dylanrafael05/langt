using System.Text;
using Langt.Structure;
using Langt.Structure.Visitors;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using Langt.Utility;
using Results;

using Spectre.Console;
using Langt.Message;

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
public abstract record SourcedTreeNode<TChild> : ISourceRanged, ITreeRenderable where TChild : SourcedTreeNode<TChild>, ISourceRanged
{
    public abstract TreeItemContainer<TChild> ChildContainer {get;}

    public IEnumerable<TChild?> AllChildren {get; init;}
    public IEnumerable<TChild> Children {get; init;}


    public virtual SourceRange Range {get; init;}
    public string DebugSourceName {get; init;}

    public virtual TreeBuilder ToStringTree()
    {
        var tree = TreeBuilder.From($"[italic]{Markup.Escape(GetType().ReadableName())}[/]");
        
        foreach(var c in ChildContainer) 
        {
            var cs = c.ChildValues;

            if(cs.Count() == 1)
            {
                var gen = c.ChildValues.First()
                    ?.ToStringTree()
                    ?? TreeBuilder.From("[gray bold italic]<null>[/]");
                
                gen.ModifyContent(s => $"[blue]{Markup.Escape(c.ChildName)}[/] [gray]=[/] {s}");
                
                tree.AddNode(gen);
            }
            else 
            {
                var gen = TreeBuilder.From($"[blue]{Markup.Escape(c.ChildName)}[/] [gray]=[/] ");

                if(!cs.Any())
                {
                    gen.ModifyContent(c => c + "[gray bold italic]<empty>[/]");
                }
                else 
                {
                    gen.ModifyContent(c => c + "[gray]...[/]");
                    foreach(var (k, ch) in cs.Indexed())
                    {
                        var t = ch?.ToStringTree() ?? TreeBuilder.From("[gray bold italic]<null>[/]");
                        t.ModifyContent(s => $"[blue]{Markup.Escape($"[{k}]")}[/] [gray]=[/] " + s);

                        gen.AddNode(t);
                    }
                }

                tree.AddNode(gen);
            }
        }

        return tree;
    }

    public SourcedTreeNode()
    {
        var cc = ChildContainer;

        AllChildren = cc.AllChildren.ToArray();
        Children    = cc.Children.ToArray();

        Range = SourceRange.CombineFrom(Children.Select(x => (ISourceRanged)x));
        DebugSourceName = GetType().ReadableName() + "@" + Range.CharStart + ":line " + Range.LineStart;
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
}

public record BoundConversion(BoundASTNode Source, LangtConversion Conversion) : BoundASTNode(Source.ASTSource)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {Source};

    public override LangtType Type => Conversion.Output;
}


public abstract record BoundASTNode(ASTNode ASTSource) : SourcedTreeNode<BoundASTNode>
{
    public override SourceRange Range => ASTSource.Range;
    
    /// <summary>
    /// Is the current AST node representative of an expression which can automatically
    /// be dereferenced or be assigned to?
    /// </summary>
    public virtual bool IsAssignable => false;
    
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
    public virtual IResolvable? Resolution {get; init;}

    /// <summary>
    /// Attempt to apply a dereference if this node is an l-value and is a pointer.
    /// </summary>
    /// <seealso cref="IsAssignable"/>
    /// <seealso cref="LangtType.IsPointer"/>
    public BoundASTNode TryDeference()
    {
        if(!Type.IsReference) return this;
        
        return new BoundConversion(this, LangtConversion.DereferenceFor(Type));
    }

    public Result<BoundASTNode> MatchExprType(Context ctx, LangtType target, out bool coerced)
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
                        .WithDgnError(Messages.Get("fn-ref-impossible"), Range)
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

        // Return here if types match
        if(target == Type) return builder.Build(this);
        
        // Beyond this point, coersion takes place
        coerced = true;

        // Attempt to find a conversion
        var convResult = ctx.ResolveConversion(Type, target, Range);
        if(!convResult) return builder.WithData(convResult).BuildError<BoundASTNode>()
                        .AsTargetTypeDependent();

        var conv = convResult.Value;

        // Refuse non-implicit conversions
        if(!conv.IsImplicit)
        {
            return builder
                .WithDgnError(Messages.Get("conversion-no-found-explicit", conv.Input, conv.Output), Range)
                .BuildError<BoundASTNode>()
                .AsTargetTypeDependent()
            ;
        }

        // Return with conversion
        var res = new BoundConversion(this, conv);
        return builder.Build<BoundASTNode>(res)
            .AsTargetTypeDependent();
    }
    public Result<BoundASTNode> MatchExprType(Context ctx, LangtType target) 
        => MatchExprType(ctx, target, out _);
}

public record BoundEmpty(ASTNode Source) : BoundASTNode(Source)
{
    public override TreeItemContainer<BoundASTNode> ChildContainer => new() {};
}

/// <summary>
/// Represents any construct directly present in the AST.
/// </summary>
public abstract record ASTNode : SourcedTreeNode<ASTNode>
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


    public virtual Result HandleDefinitions(Context ctx) => Result.Success();
    public virtual Result RefineDefinitions(Context ctx) => Result.Success();

    protected virtual Result<BoundASTNode> BindSelf(Context ctx, TypeCheckOptions options) 
        => Result.Success(new BoundEmpty(this) as BoundASTNode);

    public Result<BoundASTNode> Bind(Context ctx, TypeCheckOptions? optionsMaybe = null) 
    {
        var options = optionsMaybe ?? new();

        var result = BindSelf(ctx, options);
        if(!result) return result;

        var bast = result.Value;

        // NOTE: does not bind to a variable reference if target type dependent; is this correct?
        if(!result.GetBindingOptions().TargetTypeDependent && bast.HasResolution && bast.Resolution is LangtVariable v) 
        {
            bast = new BoundVariableReference(bast, v);
            v.UseCount++;
        }

        if(options.AutoDeference) 
            bast = bast.TryDeference();

        return result.Map(_ => bast);
    }

    public Result<BoundASTNode> BindMatchingExprType(Context ctx, LangtType type, out bool coerced, TypeCheckOptions? optionsMaybe = null)
    {
        var options = optionsMaybe ?? new();

        coerced = false;

        var binding = Bind(ctx, options with 
        {
            TargetType = type // TODO: should this always be passed this way?
        });

        if(!binding) return binding;
        var builder = ResultBuilder.From(binding);

        var bast = binding.Value;
        
        var nbast = bast.MatchExprType(ctx, type, out coerced);
        builder.AddData(nbast);

        if(!nbast) return builder.BuildError<BoundASTNode>();
        return builder.Build(nbast.Value);
    }
    public Result<BoundASTNode> BindMatchingExprType(Context ctx, LangtType type, TypeCheckOptions? optionsMaybe = null)
        => BindMatchingExprType(ctx, type, out _, optionsMaybe);
}
