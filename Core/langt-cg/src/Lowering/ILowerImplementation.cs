using Langt.AST;
using Langt.Utility;

namespace Langt.CG.Lowering;

public interface ILowerImplementation<T> : ILowerImplementation where T : BoundASTNode
{
    public void LowerImpl(CodeGenerator cg, T node);

    void ILowerImplementation.LowerImpl(CodeGenerator cg, BoundASTNode node)
    {
        LowerImpl(cg, (T)node!);
    }
}

public interface ILowerImplementation 
{
    static ILowerImplementation()
    {
#if DEBUG
        foreach(var t in typeof(BoundASTNode).Assembly.GetTypes().Where(t => t.BaseType == typeof(BoundASTNode)))
        {
            Expect.NonNull(DefaultFor(t), $"Bound AST node type {t.Name} must have associated lower implementation");
        }
#endif
    }

    public static void Lower(CodeGenerator cg, BoundASTNode node) 
    {
        var impl = DefaultFor(node.GetType());

        if(impl is null)
            throw new NotSupportedException($"Cannot lower {node.GetType()}");

        impl.LowerImpl(cg, node);
    }

    public static ILowerImplementation? DefaultFor<T>() where T : BoundASTNode
        => DefaultFor(typeof(T));
    public static ILowerImplementation? DefaultFor(Type t)
    {
        var inter    = typeof(ILowerImplementation<>).MakeGenericType(t);
        var typeImpl = typeof(ILowerImplementation<>)
            .Assembly
            .GetTypes()
            .FirstOrDefault(ty => ty.GetInterfaces().FirstOrDefault() == inter);

        if(typeImpl is null) return null;

        return (ILowerImplementation)Activator.CreateInstance(typeImpl)!;
    }
    
    public void LowerImpl(CodeGenerator cg, BoundASTNode node);
}
