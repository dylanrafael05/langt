using Langt.Codegen;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

/// <summary>
/// A special case of ASTNode which represents something which is known at compile time to be a namespace
/// </summary>
public abstract record ASTNamespace : ASTNode // TODO: permit only one namespace declaration per file; emit warnings for duplicate usings
{
    public abstract Result<LangtNamespace> Resolve(ASTPassState state, TypeCheckOptions? optionsMaybe = null);
    protected Result<LangtNamespace> ResolveFrom(IScope from, string name, SourceRange nameRange, [NotNullWhen(true)] bool allowDefinitions = false)
    {
        var builder = ResultBuilder.Empty();

        var nsResult = from.ResolveNamespace(name, Range);
        builder.AddData(nsResult);

        if(!nsResult)
        {
            if(allowDefinitions)
            {
                var dr = from.Define
                (
                    s => new LangtNamespace(s, name) 
                    {
                        DefinitionRange = Range
                    }, 

                    Range
                );
                
                return dr;
            }
        }

        return nsResult;
    }
}
