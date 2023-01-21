using Langt.Structure;
using Langt.Utility;

namespace Langt.CG.Bindings;

public class LangtTypeBuilder : TypeBuilder
{
    public override LLVMTypeRef BuildBuiltin(LangtType ty) => ty.Name switch 
    {
        "int8"  => CG.LLVMContext.Int8Type,
        "int16" => CG.LLVMContext.Int16Type,
        "int32" => CG.LLVMContext.Int32Type,
        "int64" => CG.LLVMContext.Int64Type,

        "real16" => CG.LLVMContext.HalfType,
        "real32" => CG.LLVMContext.FloatType,
        "real64" => CG.LLVMContext.DoubleType,

        "bool" => CG.LLVMContext.Int1Type,

        "none" => CG.LLVMContext.VoidType,

        "ptr" => CG.LLVMContext.CreatePointerType(0),

        _ => throw new NotSupportedException($"Unknown builtin {ty}")
    };

    public override LLVMTypeRef BuildAlias(LangtAliasType ty)
        => Build(ty.AliasBaseType);

    public override LLVMTypeRef BuildPointer(LangtPointerType ty)
        => Build(LangtType.Ptr);
        
    public override LLVMTypeRef BuildReference(LangtReferenceType ty)
        => Build(LangtType.Ptr);

    public override LLVMTypeRef BuildFunction(LangtFunctionType ty)
        => LLVMTypeRef.CreateFunction(Build(ty.ReturnType), ty.ParameterTypes.Select(CG.Binder.Get).ToArray(), ty.IsVararg);

    public override LLVMTypeRef BuildStructure(LangtStructureType ty)
    {
        var s = CG.LLVMContext.CreateNamedStruct(CodeGenerator.LangtIdentifierPrepend + ty.FullName);
        s.StructSetBody(ty.Fields.Select(f => f.Type).Select(CG.Binder.Get).ToArray(), false);

        return s;
    }

    public override LLVMTypeRef BuildOption(LangtOptionType ty)
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