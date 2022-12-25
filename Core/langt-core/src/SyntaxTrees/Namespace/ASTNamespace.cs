using Langt.Codegen;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

/// <summary>
/// A special case of ASTNode which represents something which is known at compile time to be a namespace
/// </summary>
public abstract record ASTNamespace : ASTNode // TODO: permit only one namespace declaration per file; emit warnings for duplicate usings
{
    public abstract Result<LangtNamespace> Resolve(ASTPassState state, bool allowDefinitions = false);
    
    protected Result<LangtNamespace> ResolveFrom(LangtScope from, string name, ASTPassState state, [NotNullWhen(true)] bool allowDefinitions = false)
    {
        var nsResult = from.ResolveNamespace(name, Range);

        if(!nsResult)
        {
            if(allowDefinitions)
            {
                var ns = new LangtNamespace(name);
                var dr = from.DefineNamespace(ns, Range);
                
                return Result.Success(ns).WithDataFrom(dr);
            }
        }

        return nsResult;
    }
}
