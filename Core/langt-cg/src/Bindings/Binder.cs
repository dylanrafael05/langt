using Langt.Structure;
using Langt.Utility;

namespace Langt.CG.Bindings;

public class Binder
{
    public Binder(TypeBuilder typeBuilder)
    {
        this.typeBuilder = typeBuilder;
    }

    private TypeBuilder typeBuilder;

    private readonly Dictionary<LangtVariable, LLVMValueRef> variableBindings = new();
    private readonly Dictionary<LangtFunction, LLVMValueRef> functionBindings = new();

    private readonly Dictionary<LangtType, LLVMTypeRef> typeBindings = new();

    public LLVMTypeRef Get(LangtType ty) 
    {
        if(!typeBindings.ContainsKey(ty))
        {
            typeBindings.Add(ty, typeBuilder.Build(ty));
        }

        return typeBindings[ty];
    }

    public void BindVariable(LangtVariable variable, LLVMValueRef llvm)
    {
        Expect.Not(variableBindings.ContainsKey(variable));

        variableBindings[variable] = llvm;
    }
    public void BindFunction(LangtFunction function, LLVMValueRef llvm)
    {
        Expect.Not(functionBindings.ContainsKey(function));

        functionBindings[function] = llvm;
    }

    public LLVMValueRef Get(LangtVariable variable)
        => variableBindings[variable];
    public LLVMValueRef Get(LangtFunction function)
        => functionBindings[function];
}
