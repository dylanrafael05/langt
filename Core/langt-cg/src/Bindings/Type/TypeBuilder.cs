using Langt.Structure;
using Langt.Utility;

namespace Langt.CG.Bindings;

public class TypeBuilder : Builder<LangtType, LLVMTypeRef>
{
    public override LLVMTypeRef Build(LangtType ty) => ty switch 
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

    public LLVMTypeRef BuildBuiltin(LangtType ty) => ty.Name switch 
    {
        LangtWords.Integer8  => CG.LLVMContext.Int8Type,
        LangtWords.Integer16 => CG.LLVMContext.Int16Type,
        LangtWords.Integer32 => CG.LLVMContext.Int32Type,
        LangtWords.Integer64 => CG.LLVMContext.Int64Type,

        LangtWords.Real16 => CG.LLVMContext.HalfType,
        LangtWords.Real32 => CG.LLVMContext.FloatType,
        LangtWords.Real64 => CG.LLVMContext.DoubleType,

        LangtWords.Boolean => CG.LLVMContext.Int1Type,

        LangtWords.Unit => CG.LLVMContext.VoidType,

        LangtWords.Pointer => CG.LLVMContext.CreatePointerType(0),

        _ => throw new NotSupportedException($"Unknown builtin {ty}")
    };

    public LLVMTypeRef BuildAlias(LangtAliasType ty)
        => Build(ty.AliasBaseType!);

    public LLVMTypeRef BuildPointer(LangtPointerType ty)
        => Build(LangtType.Ptr);
        
    public LLVMTypeRef BuildReference(LangtReferenceType ty)
        => Build(LangtType.Ptr);

    public LLVMTypeRef BuildFunction(LangtFunctionType ty)
        => LLVMTypeRef.CreateFunction(Build(ty.ReturnType), ty.ParameterTypes.Select(CG.Binder.Get).ToArray(), ty.IsVararg);

    public LLVMTypeRef BuildStructure(LangtStructureType ty)
    {
        var s = CG.LLVMContext.CreateNamedStruct(CodeGenerator.LangtIdentifierPrepend + ty.FullName);
        s.StructSetBody(ty.Fields.Select(f => f.Type).Select(CG.Binder.Get).ToArray(), false);

        return s;
    }

    public LLVMTypeRef BuildOption(LangtOptionType ty)
    {
        var maxSize = ty.OptionTypes.Max(CG.Sizeof);

        var elems = new List<LLVMTypeRef>
        {
            LLVMTypeRef.CreateArray(LLVMTypeRef.Int8, (uint)maxSize),
            LLVMTypeRef.Int8 // Tag
        };

        var s = CG.LLVMContext.CreateNamedStruct(ty.FullName);
        s.StructSetBody(elems.ToArray(), false);

        return s;
    }
}