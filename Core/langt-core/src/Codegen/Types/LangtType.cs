using Langt.Structure.Visitors;
using Langt.Codegen;
using Langt.Utility;
using System.Diagnostics.CodeAnalysis;

namespace Langt.Codegen;

public abstract class LangtResolvableType : LangtType, IResolution
{
    public LangtResolvableType(string name, IScope scope) : base(name)
    {
        HoldingScope = scope;
    }

    public required SourceRange? DefinitionRange {get; init;}
    public IScope HoldingScope {get;}
}

public abstract class LangtType : INamed
{
    public LangtType(string name)
    {
        Name = name;
    }
    public LangtType()
    {
        Name = "";
    }

    public virtual string Name {get; init;}
    public virtual string DisplayName => Name;
    public virtual string FullName => Name;
    
    public virtual string? Documentation {get; init;} = null;

    public int? IntegerBitDepth {get; init;}
    public int? RealBitDepth {get; init;}

    public bool IsInteger => IntegerBitDepth != null;
    public bool IsReal => RealBitDepth != null;

    public virtual LangtType? PointeeType => null;
    public bool IsPointer => PointeeType != null;

    public bool IsStructure => this is LangtStructureType;
    public LangtStructureType? Structure => this as LangtStructureType;

    public bool IsFunction => this is LangtFunctionType;
    public bool IsFunctionPtr => IsPointer && PointeeType!.IsFunction;

    public bool IsAlias => this is LangtAliasType;
    public virtual LangtType? AliasBaseType => null;

    public virtual bool IsBuiltin => false;

    public abstract LLVMTypeRef Lower(CodeGenerator context);

    public class Wrapper : LangtType
    {
        private LLVMTypeRef llvm;
        public Wrapper(string name, LLVMTypeRef llvm) : base(name)
        {
            this.llvm = llvm;
        }
        
        public override LLVMTypeRef Lower(CodeGenerator context) => llvm;
        public override bool IsBuiltin => true;
    }

    [BuiltinType] public static readonly LangtType Real64 = new Wrapper("real64", LLVMTypeRef.Double) {RealBitDepth = 64};
    [BuiltinType] public static readonly LangtType Real32 = new Wrapper("real32", LLVMTypeRef.Float) {RealBitDepth = 32};
    [BuiltinType] public static readonly LangtType Real16 = new Wrapper("real16", LLVMTypeRef.Half) {RealBitDepth = 16};
    [BuiltinType] public static readonly LangtType Int64  = new Wrapper("int64" , LLVMTypeRef.Int64) {IntegerBitDepth = 64};
    [BuiltinType] public static readonly LangtType Int32  = new Wrapper("int32" , LLVMTypeRef.Int32) {IntegerBitDepth = 32};
    [BuiltinType] public static readonly LangtType Int16  = new Wrapper("int16" , LLVMTypeRef.Int16) {IntegerBitDepth = 16};
    [BuiltinType] public static readonly LangtType Int8   = new Wrapper("int8"  , LLVMTypeRef.Int8) {IntegerBitDepth = 8};
    [BuiltinType] public static readonly LangtType Bool   = new Wrapper("bool"  , LLVMTypeRef.Int1);
    [BuiltinType] public static readonly LangtType None   = new Wrapper("none"  , LLVMTypeRef.Void);
    [BuiltinType] public static readonly LangtType Ptr    = new Wrapper("ptr", LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0));

    public static readonly LangtType[] IntegerTypes = {Int8, Int16, Int32, Int64};
    public static readonly LangtType[] RealTypes    = {Real16, Real32, Real64};

    public static readonly LangtType[] BuiltinTypes =
    {
        Real64, Real32, Real16,
        Int64, Int32, Int16, Int8,
        Bool,
        None,
        Ptr
    };

    public static readonly LangtType Error = new Wrapper("error", LLVMTypeRef.Void);

    public bool IsError => ReferenceEquals(this, Error);
}
