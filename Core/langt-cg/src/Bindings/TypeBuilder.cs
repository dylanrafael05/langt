using Langt.Structure;

namespace Langt.CG.Bindings;

public abstract class TypeBuilder 
{
    public required CodeGenerator CG {get; init;}

    public LLVMTypeRef Build(LangtType ty) => ty switch 
    {
        {IsBuiltin:   true} => BuildBuiltin(ty),
        {IsPointer:   true} => BuildPointer(ty.Pointer),
        {IsReference: true} => BuildReference(ty.Reference),
        {IsFunction:  true} => BuildFunction(ty.Function),
        {IsStructure: true} => BuildStructure(ty.Structure),
        {IsAlias:     true} => BuildAlias(ty.Alias),
        {IsOption:    true} => BuildOption(ty.Option),

        _ => throw new NotImplementedException($"Cannot build type {ty}")
    };

    public abstract LLVMTypeRef BuildBuiltin(LangtType ty);
    public abstract LLVMTypeRef BuildPointer(LangtPointerType ty);
    public abstract LLVMTypeRef BuildReference(LangtReferenceType ty);
    public abstract LLVMTypeRef BuildFunction(LangtFunctionType ty);
    public abstract LLVMTypeRef BuildStructure(LangtStructureType ty);
    public abstract LLVMTypeRef BuildAlias(LangtAliasType ty);
    public abstract LLVMTypeRef BuildOption(LangtOptionType ty);
}
