using Langt.Structure.Visitors;
using Langt.Codegen;
using Langt.Utility;
using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;
public abstract record LangtType(string Name) : INamedScoped
{
    public int? IntegerBitDepth {get; init;}
    public int? RealBitDepth {get; init;}

    public bool IsInteger => IntegerBitDepth != null;
    public bool IsReal => RealBitDepth != null;

    public virtual LangtType? PointeeType => null;
    public bool IsPointer => PointeeType != null;

    public bool IsStructure => this is LangtStructureType;
    public LangtStructureType? Structure => this as LangtStructureType;

    public bool IsAlias => this is LangtAliasType;
    public virtual LangtType? AliasBaseType => null;
    
    public LangtScope? HoldingScope { get; set; }

    public abstract LLVMTypeRef Lower(CodeGenerator context);

    public record Wrapper(string Name, LLVMTypeRef LLVMType) : LangtType(Name)
    {
        public override LLVMTypeRef Lower(CodeGenerator context) => LLVMType;
    }

    [BuiltinType] public static readonly LangtType Real64 = new Wrapper("real64", LLVMTypeRef.Double) {RealBitDepth = 64};
    [BuiltinType] public static readonly LangtType Real32 = new Wrapper("real32", LLVMTypeRef.Float) {RealBitDepth = 32};
    [BuiltinType] public static readonly LangtType Int64  = new Wrapper("int64" , LLVMTypeRef.Int64) {IntegerBitDepth = 64};
    [BuiltinType] public static readonly LangtType Int32  = new Wrapper("int32" , LLVMTypeRef.Int32) {IntegerBitDepth = 32};
    [BuiltinType] public static readonly LangtType Int16  = new Wrapper("int16" , LLVMTypeRef.Int16) {IntegerBitDepth = 16};
    [BuiltinType] public static readonly LangtType Int8   = new Wrapper("int8"  , LLVMTypeRef.Int8) {IntegerBitDepth = 8};

    [BuiltinType] public static readonly LangtType Bool   = new Wrapper("bool"  , LLVMTypeRef.Int1);

    [BuiltinType] public static readonly LangtType None   = new Wrapper("none"  , LLVMTypeRef.Void);

    public static readonly LangtType Error = new Wrapper("error", LLVMTypeRef.Void);

    public bool IsError => ReferenceEquals(this, Error);

    public static LangtType PointerTo(LangtType type)
        => new LangtPointerType(type);
    public static LangtType Function(LangtType returnType, params LangtType[] paramTypes)
        => new LangtFunctionType(returnType, paramTypes);
    public static LangtType Function(LangtType returnType, bool vararg, params LangtType[] paramTypes)
        => new LangtFunctionType(returnType, vararg, paramTypes);
    public static LangtType FunctionVarargs(LangtType returnType, params LangtType[] paramTypes)
        => new LangtFunctionType(returnType, true, paramTypes);
    // TODO: LangtType.Struct()
}
