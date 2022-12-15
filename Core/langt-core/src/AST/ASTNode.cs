using System.Text;
using Langt.Structure;
using Langt.Codegen;
using Langt.Structure.Visitors;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

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

public record struct TypeCheckState(CodeGenerator CG, LangtType TargetType, bool TryRead, bool ProduceDiagnostics)
{
    public static TypeCheckState Start(CodeGenerator gen) => new(gen, LangtType.None, true, true);
}

[Serializable]
public class TypeCheckException : Exception
{}

/// <summary>
/// Represents any construct directly present in the AST.
/// </summary>
public abstract record ASTNode : ISourceRanged, IElement<VisitDumper>
{
    /// <summary>
    /// Get the child container which describes all children of this AST node.
    /// </summary>
    public abstract ASTChildContainer ChildContainer {get;}

    /// <summary>
    /// Get the sequence of all children in order. These children may be null.
    /// </summary>
    public IEnumerable<ASTNode?> AllChildren {get; private init;}
    /// <summary>
    /// Get the sequence of all non-null children in order.
    /// </summary>
    public IEnumerable<ASTNode> Children {get; private init;}

    public ASTNode()
    {
        var cc = ChildContainer;

        AllChildren = cc.AllChildren.ToArray();
        Children    = cc.Children.ToArray();

        Range = SourceRange.CombineFrom(Children);
        DebugSourceName = GetType().Name + "@" + Range.CharStart + ":line " + Range.LineStart;
    }

    /// <summary>
    /// Get the source range that contains the entirety of this AST node.
    /// </summary>
    public virtual SourceRange Range {get; private init;}
    
    /// <summary>
    /// Is the current AST node representative of an expression which can automatically
    /// be dereferenced or be assigned to if it constitutes a pointer?
    /// </summary>
    public virtual bool IsLValue => false;
    /// <summary>
    /// Is the current AST node block-like; should the presence of an invalid child 
    /// prevent type checking from taking place on this node?
    /// </summary>
    public virtual bool BlockLike => false;
    /// <summary>
    /// Is the current AST node untypable; does this AST node require a provided valid
    /// target type in order to type check?
    /// </summary>
    public virtual bool RequiresTypeDownflow => false;

    public string DebugSourceName {get; private init;}

    protected virtual void ResetDownflowState(CodeGenerator generator) {}
    protected virtual bool AcceptDownflowRaw(LangtType? type, CodeGenerator generator, bool err) => true;

    /// <summary>
    /// Updates this AST node so that it corresponds to the given target type.
    /// Returns <see langword="true"/> of no errors were reported, 
    /// and <see langword="false"/> otherwise.
    /// Note that implementers should ensure this method can be called multiple
    /// times, but can only produce a valid expression state if this returns <see langword="true"/>.
    /// </summary>
    public bool AcceptDownflow(LangtType? type, CodeGenerator generator, bool err = true) 
    {
        ResetDownflowState(generator);
        return HasValidDownflow = AcceptDownflowRaw(type, generator, err);
    }

    /// <summary>
    /// Whether or not the current AST node has an unknown type, since it has not been 
    /// assigned a valid target type.
    /// </summary>
    public bool HasValidDownflow {get; set;} = false;
    
    /// <summary>
    /// What is the direct type (unaffected by transformers) of this AST node. Should be
    /// equal to <see cref="LangtType.Error"/> if this node is not an expression.
    /// </summary>
    protected LangtType RawExpressionType {get; set;} = LangtType.Error;
    /// <summary>
    /// What is the inferabble type of this AST node; what type should be used for type
    /// inference of this expression if it is distinct from <see cref="RawExpressionType"/>.
    /// Should be equal to <see langword="null"/> if this node is not an expression, or
    /// there is no distinction between the type to be inferred or the expression type itself. 
    /// </summary>
    public LangtType? NaturalType {get; protected set;} = null;
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
    
    /// <summary>
    /// Whether or not this AST node represents an identifier for a statically resolveable
    /// item.
    /// </summary>
    /// <seealso cref="INamedScoped"/>
    public bool HasResolution {get; set;}
    /// <summary>
    /// The statically resolveable item this AST node represents an identifier for.
    /// Will not be <see langword="null"/> if <see cref="HasResolution"/> returns true.
    /// </summary>
    /// <seealso cref="HasResolution"/>
    /// <seealso cref="INamedScoped"/>
    [MemberNotNullWhen(true, nameof(HasResolution))]
    public INamedScoped? Resolution {get; set;}

    /// <summary>
    /// The list of transformers currently applied to this AST node, in reverse order 
    /// of their application.
    /// </summary>
    public List<ITransformer> AppliedTransformers {get; private init;} = new();

    public bool PassedInitialTypeCheck {get; private set;} = false;

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
    public void TryRead()
    {
        if(!IsLValue || !TransformedType.IsPointer) return;
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

    public void Error(TypeCheckState state, string message)
    {
        if(state.ProduceDiagnostics) state.CG.Diagnostics.Error(message, Range);
        throw new TypeCheckException();
    }
    public void Warning(TypeCheckState state, string message)
    {
        if(state.ProduceDiagnostics) state.CG.Diagnostics.Warning(message, Range);
    }
    public void Note(TypeCheckState state, string message)
    {
        if(state.ProduceDiagnostics) state.CG.Diagnostics.Note(message, Range);
    }

    /// <summary>
    /// Perform the initial steps of type checking.
    /// Errors in this step should generally be considered fatal or unavoidable.
    /// The given "target type" from the state may be null.
    /// </summary>
    protected virtual void InitialTypeCheckSelf(TypeCheckState state) 
    {
        Error(state, "Cannot yet type-check node of type " + GetType().Name);
    }
    /// <summary>
    /// Perform the final steps of type checking.
    /// This method, under certain circumstances where multiple type-checks must occur 
    /// (function overload resolution namely).
    /// </summary>
    protected virtual void TypeCheckSelf(TypeCheckState state)
    {}

    private void TypeCheckInternal(TypeCheckState state)
    {
        TypeCheckSelf(state);
        if(state.TryRead) TryRead();
    }

    public void TypeCheck(TypeCheckState state) 
    {
        state = state with {ProduceDiagnostics=true};

        InitialTypeCheckSelf(state);
        TypeCheckInternal(state);
    }

    public bool TryTypeCheck(TypeCheckState state)
    {
        state = state with {ProduceDiagnostics=false};

        if(!BlockLike && ContainsInvalid)
        {
            return true; // TODO: is this valid?
        }

        try 
        {
            InitialTypeCheckSelf(state);
            TypeCheckInternal(state);
        }
        catch(TypeCheckException)
        {
            return false;
        }

        return true;
    }

    public virtual void LowerSelf(CodeGenerator generator)
        => generator.Logger.Note("Cannot yet lower AST Node of type " + GetType().Name);
    public void Lower(CodeGenerator generator)
    {   
        generator.Project.Logger.Debug("Lowering " + DebugSourceName, "lowering");

        if(Unreachable)
        {
            generator.Project.Diagnostics.Warning("Unreachable code detected", Range);
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
