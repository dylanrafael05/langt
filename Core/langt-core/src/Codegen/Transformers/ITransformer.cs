using System.Text;
using Langt.Codegen;

namespace Langt.AST;

/// <summary>
/// A transformer is an operation inserted by the compiler which modifies some value, changing its type. 
/// <br/>
/// Transforms should only be applied to nodes after their TypeCheck calls have been made.
/// </summary>
public interface ITransformer
{
    LangtType Input {get;}
    LangtType Output {get;}
    string Name => Input.RawName + "->" + Output.RawName;

    LLVMValueRef Perform(CodeGenerator generator, LLVMValueRef value);
}

public delegate LLVMValueRef TransformFunction(CodeGenerator generator, LLVMValueRef value);
public delegate LLVMValueRef TransformProviderFunction(LangtType Input, LangtType Output, CodeGenerator generator, LLVMValueRef value);
public delegate bool TransformCanPerformPredicate(LangtType input, LangtType result);
