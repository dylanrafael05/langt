using Langt.Structure;
using Langt.Structure.Resolutions;
using System.Diagnostics.CodeAnalysis;

namespace Langt.AST;

/// <summary>
/// A special case of ASTNode which represents something which is known at compile time to be a namespace
/// </summary>
public abstract record ASTNamespace : ASTNode, ISymbolProvider<Namespace> // TODO: permit only one namespace declaration per file; emit warnings for duplicate usings
{
    public abstract ISymbol<Namespace> GetSymbol(Context ctx);

    // public abstract Result<LangtNamespace> Resolve(Context ctx, TypeCheckOptions? optionsMaybe = null);
    // protected Result<LangtNamespace> ResolveFrom(IScope from, string name, SourceRange? nameRange = null, [NotNullWhen(true)] bool allowDefinitions = false)
    // {
    //     var builder = ResultBuilder.Empty();

    //     var nsResult = from.ResolveNamespace(name, Range);
    //     builder.AddData(nsResult);

    //     if(!nsResult)
    //     {
    //         if(allowDefinitions)
    //         {
    //             var dr = from.Define
    //             (
    //                 s => new LangtNamespace(s, name) 
    //                 {
    //                     DefinitionRange = nameRange ?? Range
    //                 }, 

    //                 Range
    //             );
                
    //             return dr;
    //         }
    //     }

    //     return nsResult;
    // }
}
