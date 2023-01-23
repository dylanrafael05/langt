using Langt.Structure.Visitors;
using Langt.Structure;
using Langt.Utility;
using System.Diagnostics.CodeAnalysis;
using Langt.Structure.Resolutions;

namespace Langt.Structure;

public abstract class LangtResolvableType : LangtType, IResolution
{
    public LangtResolvableType(string name, IScope scope) : base(name)
    {
        HoldingScope = scope;
    }

    public required SourceRange? DefinitionRange {get; init;}
    public new IScope HoldingScope {get;}
}

public abstract class LangtType : INamed, IEquatable<LangtType>
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

    [MemberNotNullWhen(true, nameof(ElementType)), MemberNotNullWhen(true, nameof(Pointer))] 
    public bool IsPointer => this is LangtPointerType;
    public LangtPointerType? Pointer => this as LangtPointerType;
    [MemberNotNullWhen(true, nameof(ElementType)), MemberNotNullWhen(true, nameof(Reference))] 
    public bool IsReference => this is LangtReferenceType;
    public LangtReferenceType? Reference => this as LangtReferenceType;
    public virtual LangtType? ElementType => null;

    [MemberNotNullWhen(true, nameof(Structure))] public bool IsStructure => this is LangtStructureType;
    public LangtStructureType? Structure => this as LangtStructureType;

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

    public virtual bool IsBuiltin => false;

    // public abstract LLVMTypeRef Lower(Context context);


    public bool Equals(LangtType? other) 
    {
        if(other is null) return false;

        //* Expects builtins are uniquely named
        if(IsBuiltin)   return other.IsBuiltin && Name == other.Name;
        if(IsPointer)   return other.IsPointer && ElementType == other.ElementType;
        if(IsReference) return other.IsReference && ElementType == other.ElementType;
        
        //* Expects these to be the only requirements for function type equality
        if(IsFunction) return other.IsFunction
                           && Function.ReturnType == other.Function.ReturnType
                           && Function.ParameterTypes.SequenceEqual(other.Function.ParameterTypes)
                           && Function.IsVararg == other.Function.IsVararg;

        //* Expects that scopes cannot hold duplicates
        if(IsResolution) return HoldingScope == other.HoldingScope
                             && Name == other.Name;

        if(IsOption) return other.IsOption 
                         && OptionTypes.SetEquals(other.OptionTypes);

        throw new NotImplementedException($"Cannot check equality for types {GetType().FullName} and {other.GetType().FullName}");
    }

    public sealed override bool Equals(object? obj)
        => obj is LangtType lt && Equals(lt);

    public sealed override string ToString()
        => FullName;

    // TODO: reimpl? optimize FullName?
    public override int GetHashCode()
        => FullName.GetHashCode();

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
    }

    public static readonly LangtType Real64 = new BasicType(LangtWords.Real64)    {RealBitDepth = 64};
    public static readonly LangtType Real32 = new BasicType(LangtWords.Real32)    {RealBitDepth = 32};
    public static readonly LangtType Real16 = new BasicType(LangtWords.Real16)    {RealBitDepth = 16};
    public static readonly LangtType Int64  = new BasicType(LangtWords.Integer64) {IntegerBitDepth = 64};
    public static readonly LangtType Int32  = new BasicType(LangtWords.Integer32) {IntegerBitDepth = 32};
    public static readonly LangtType Int16  = new BasicType(LangtWords.Integer16) {IntegerBitDepth = 16};
    public static readonly LangtType Int8   = new BasicType(LangtWords.Integer8)  {IntegerBitDepth = 08};
    public static readonly LangtType Bool   = new BasicType(LangtWords.Boolean);
    public static readonly LangtType None   = new BasicType(LangtWords.Unit);
    public static readonly LangtType Ptr    = new BasicType(LangtWords.Pointer);

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

    public static readonly LangtType Error = new BasicType(LangtWords.Error);

    public bool IsError => ReferenceEquals(this, Error);
}
