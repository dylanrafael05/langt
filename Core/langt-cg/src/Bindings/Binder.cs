using Langt.Structure;
using Langt.Utility;

namespace Langt.CG.Bindings;

public class Binder
{
    public Binder(Builder<LangtType, LLVMTypeRef> typeBuilder, Builder<LangtFunction, LLVMValueRef> funcBuilder, Builder<LangtConversion, CodeGenerator.Applicator> convBuilder)
    {
        this.typeBuilder = typeBuilder;
        this.funcBuilder = funcBuilder;
        this.convBuilder = convBuilder;
    }

    private readonly Builder<LangtType, LLVMTypeRef> typeBuilder;
    private readonly Builder<LangtFunction, LLVMValueRef> funcBuilder;
    private readonly Builder<LangtConversion, CodeGenerator.Applicator> convBuilder;

    private readonly Dictionary<LangtVariable, LLVMValueRef> variableBindings = new();
    private readonly Dictionary<LangtFunction, LLVMValueRef> functionBindings = new();
    private readonly Dictionary<LangtConversion, CodeGenerator.Applicator> conversionBindings = new();

    private readonly Dictionary<LangtType, LLVMTypeRef> typeBindings = new();

    public LLVMTypeRef Get(LangtType ty) 
    {
        if(!typeBindings.ContainsKey(ty))
        {
            typeBindings.Add(ty, typeBuilder.Build(ty));
        }

        return typeBindings[ty];
    }
    
    public LLVMValueRef Get(LangtFunction fn)
    {
        if(!functionBindings.ContainsKey(fn))
        {
            functionBindings.Add(fn, funcBuilder.Build(fn));
        }

        return functionBindings[fn];
    }

    public CodeGenerator.Applicator Get(LangtConversion cv)
    {
        if(!conversionBindings.ContainsKey(cv))
        {
            conversionBindings.Add(cv, convBuilder.Build(cv));
        }

        return conversionBindings[cv];
    }

    public void BindVariable(LangtVariable variable, LLVMValueRef llvm)
    {
        Expect.Not(variableBindings.ContainsKey(variable), "Cannot bind an already bound variable!");

        variableBindings[variable] = llvm;
    }

    public LLVMValueRef Get(LangtVariable variable)
        => variableBindings[variable];
}
