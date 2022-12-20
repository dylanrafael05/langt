using Langt.Codegen;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

/// <summary>
/// A special case of ASTNode which represents something which is known at compile time to be a namespace
/// </summary>
public abstract record ASTNamespace : ASTNode // TODO: permit only one namespace declaration per file; emit warnings for duplicate usings
{
    public abstract LangtNamespace? Resolve(ASTPassState state, bool allowDefinitions = false);
    
    protected LangtNamespace? ResolveFrom(LangtScope from, string name, ASTPassState state, [NotNullWhen(true)] bool allowDefinitions = false)
    {
        var ns = from.ResolveNamespace(name, Range, state with {Noisy = false});

        if(ns is null)
        {
            if(allowDefinitions)
            {
                ns = new(name);
                from.DefineNamespace(ns, Range, state);
                return ns;
            }
            else
            {
                state.Error($"Could not resolve namespace {name}", Range);
                return null;
            }
        }

        return ns;
    }
}
