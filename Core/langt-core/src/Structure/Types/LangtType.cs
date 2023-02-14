using Langt.Structure.Visitors;
using Langt.Structure;
using Langt.Utility;
using System.Diagnostics.CodeAnalysis;
using Langt.Structure.Resolutions;
using Langt.AST;

namespace Langt.Structure;

public abstract class LangtResolvableType : LangtType, IResolution
{
    public LangtResolvableType(string name, IScope scope) : base(name)
    {
        HoldingScope = scope;
    }

    public required SourceRange? DefinitionRange {get; init;}
    public new IScope HoldingScope {get;}

    public override bool Equals(LangtType? other)
        => other is not null 
        && other.IsResolution 
        && Name == other.Name 
        && HoldingScope == other.HoldingScope;
}

public abstract class LangtType : IFullNamed, IEquatable<LangtType>
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
    public Signedness? Signedness {get; init;}

    [MemberNotNullWhen(true, nameof(IntegerBitDepth))]
    public bool IsNativeInteger => IntegerBitDepth is -1;

    [MemberNotNullWhen(true, nameof(IntegerBitDepth)), MemberNotNullWhen(true, nameof(Signedness))]
    public bool IsInteger => IntegerBitDepth != null;

    [MemberNotNullWhen(true, nameof(RealBitDepth))]
    public bool IsReal => RealBitDepth != null;

    [MemberNotNullWhen(true, nameof(ElementType)), MemberNotNullWhen(true, nameof(Pointer))] 
    public bool IsPointer => this is LangtPointerType;
    public bool IsAnyPointer => this is LangtPointerType || this == Ptr;
    public LangtPointerType? Pointer => this as LangtPointerType;
    [MemberNotNullWhen(true, nameof(ElementType)), MemberNotNullWhen(true, nameof(Reference))] 
    public bool IsReference => this is LangtReferenceType;
    public LangtReferenceType? Reference => this as LangtReferenceType;
    public virtual LangtType? ElementType => null;

    [MemberNotNullWhen(true, nameof(Structure))] public bool IsStructure => this is IStructureType;
    public IStructureType? Structure => this as IStructureType;

    [MemberNotNullWhen(true, nameof(Function))] public bool IsFunction => this is LangtFunctionType;
    public LangtFunctionType? Function => this as LangtFunctionType;
    public bool IsFunctionPtr => IsPointer && ElementType!.IsFunction;

    [MemberNotNullWhen(true, nameof(AliasBaseType)), MemberNotNullWhen(true, nameof(Alias))]
    public bool IsAlias => this is LangtAliasType;
    public LangtAliasType? Alias => this as LangtAliasType;
    public virtual LangtType? AliasBaseType => null;


    [MemberNotNullWhen(true, nameof(OptionTypes)), MemberNotNullWhen(true, nameof(OptionTypeMap)), MemberNotNullWhen(true, nameof(Option))] 
    public bool IsOption => this is LangtOptionType;
    public LangtOptionType? Option => this as LangtOptionType;
    public virtual IReadOnlySet<LangtType>? OptionTypes => null;
    public virtual IReadOnlyDictionary<LangtType, int>? OptionTypeMap => null;

    [MemberNotNullWhen(true, nameof(HoldingScope))] public bool IsResolution => this is IResolution;
    public IScope? HoldingScope => (this as IResolution)?.HoldingScope;

    [MemberNotNullWhen(true, nameof(HoldingScope))] public bool IsGenericParameter => this is LangtGenericParameterType;

    public virtual bool IsConstructed => true;
    public virtual IReadOnlyList<LangtType> GenericParameters => Array.Empty<LangtType>();
    
    public virtual bool IsBuiltin => false;

    /// <summary>
    /// Replace a given parameter type with another; should only be called in generation code.
    /// Any errors will be failed out with.
    /// </summary>
    public virtual LangtType ReplaceGeneric(LangtType ty, LangtType newty)
    {
        return this == ty ? newty : this;
    }

    /// <summary>
    /// Replace a all parameters with the given types.
    /// </summary>
    public LangtType ReplaceAllGenerics(IReadOnlyList<LangtType> ty, IReadOnlyList<LangtType> newty)
    {
        Expect.That(ty.Count == newty.Count, "Input and output replacements must be of equal length");

        var r = this;
        for(int i = 0; i < ty.Count; i++)
        {
            r = r.ReplaceGeneric(ty[i], newty[i]);
        }

        return r;
    }

    /// <summary>
    /// Check if this type contains any references to the given type
    /// </summary>
    public virtual bool Contains(LangtType ty)
    {
        return this == ty;
    }

    public Result<LangtType> RequireConstructed(SourceRange range = default)
    {
        if(IsConstructed) return Result.Success(this);

        return Result.Error<LangtType>(Diagnostic.Error($"Cannot use unconstructed type {this} in this context", range));
    }

    public abstract bool Equals(LangtType? other);

    public sealed override bool Equals(object? obj)
        => obj is LangtType lt && Equals(lt);

    public sealed override string ToString()
        => FullName;

    public override int GetHashCode()
        => HashCode.Combine(Name, HoldingScope);

    public static bool operator ==(LangtType? a, LangtType? b) 
        => a is null ? b is null : a.Equals(b);
    public static bool operator !=(LangtType? a, LangtType? b) 
        => !(a == b);


    public class BasicType : LangtType
    {
        public BasicType(string name) : base(name)
        {}
        
        // public override LLVMTypeRef Lower(Context context) => llvm;
        public override bool IsBuiltin => true;

        public override bool Equals(LangtType? other)
            => other is not null
            && other.IsBuiltin
            && Name == other.Name;
    }

    public static readonly LangtType Real64 = new BasicType(LangtWords.Real64) {RealBitDepth = 64};
    public static readonly LangtType Real32 = new BasicType(LangtWords.Real32) {RealBitDepth = 32};
    public static readonly LangtType Real16 = new BasicType(LangtWords.Real16) {RealBitDepth = 16};

    public static readonly LangtType UIntSZ = new BasicType(LangtWords.UnsignedIntegerN)  { IntegerBitDepth = -1, Signedness = Langt.Structure.Signedness.Unsigned};
    public static readonly LangtType UInt64 = new BasicType(LangtWords.UnsignedInteger64) { IntegerBitDepth = 64, Signedness = Langt.Structure.Signedness.Unsigned};
    public static readonly LangtType UInt32 = new BasicType(LangtWords.UnsignedInteger32) { IntegerBitDepth = 32, Signedness = Langt.Structure.Signedness.Unsigned};
    public static readonly LangtType UInt16 = new BasicType(LangtWords.UnsignedInteger16) { IntegerBitDepth = 16, Signedness = Langt.Structure.Signedness.Unsigned};
    public static readonly LangtType UInt8  = new BasicType(LangtWords.UnsignedInteger8)  { IntegerBitDepth = 08, Signedness = Langt.Structure.Signedness.Unsigned};

    public static readonly LangtType IntSZ = new BasicType(LangtWords.IntegerN)  { IntegerBitDepth = -1, Signedness = Langt.Structure.Signedness.Signed};
    public static readonly LangtType Int64 = new BasicType(LangtWords.Integer64) { IntegerBitDepth = 64, Signedness = Langt.Structure.Signedness.Signed};
    public static readonly LangtType Int32 = new BasicType(LangtWords.Integer32) { IntegerBitDepth = 32, Signedness = Langt.Structure.Signedness.Signed};
    public static readonly LangtType Int16 = new BasicType(LangtWords.Integer16) { IntegerBitDepth = 16, Signedness = Langt.Structure.Signedness.Signed};
    public static readonly LangtType Int8  = new BasicType(LangtWords.Integer8)  { IntegerBitDepth = 08, Signedness = Langt.Structure.Signedness.Signed};

    public static readonly LangtType Bool = new BasicType(LangtWords.Boolean);
    public static readonly LangtType None = new BasicType(LangtWords.None);
    public static readonly LangtType Ptr  = new BasicType(LangtWords.Pointer);

    public static readonly LangtType[] UnsignedIntegerTypes = {UInt8, UInt16, UInt32, UInt64, UIntSZ};
    public static readonly LangtType[] SignedIntegerTypes = {Int8, Int16, Int32, Int64, IntSZ};
    public static readonly LangtType[] AllIntegerTypes = {UInt8, UInt16, UInt32, UInt64, UIntSZ, Int8, Int16, Int32, Int64, IntSZ};
    public static readonly LangtType[] RealTypes = {Real16, Real32, Real64};

    public static readonly LangtType[] BuiltinTypes =
    {
        Real64, Real32, Real16,
        IntSZ,  Int64,  Int32,  Int16,  Int8,
        UIntSZ, UInt64, UInt32, UInt16, UInt8,
        Bool,
        None,
        Ptr
    };

    public static readonly LangtType Error = new BasicType(LangtWords.Error);

    public bool IsError => ReferenceEquals(this, Error);
}
