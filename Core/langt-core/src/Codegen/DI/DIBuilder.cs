namespace Langt.Codegen;

public enum DWARFBasicType : uint
{
    Address,
    Boolean,
    ComplexFloat,
    Float,
    Signed,
    SignedChar,
    Unsigned,
    UnsignedChar,
    ImaginaryFloat,
    PackedDecimal,
    NumericString,
    Edited,
    SignedFixed,
    UnsignedFixed,
}

public unsafe static class DIExtensions
{
    public const LLVMDIFlags DefaultFlags = LLVMDIFlags.LLVMDIFlagZero;

    private static LLVMOpaqueMetadata* ToPtr(this LLVMMetadataRef m) 
        => (LLVMOpaqueMetadata*)m.Handle;

    public static LLVMMetadataRef CreateBasicType(this LLVMDIBuilderRef self, string name, ulong size, DWARFBasicType encoding, LLVMDIFlags flags = DefaultFlags)
    {
        var mname = new MarshaledString(name);

        return new((nint)LLVM.DIBuilderCreateBasicType
        (
            (LLVMOpaqueDIBuilder*)self.Handle, 
            mname.Value, (nuint)mname.Length, size, 
            (uint)encoding, flags
        ));
    }

    public static LLVMMetadataRef CreateStructType(
        this LLVMDIBuilderRef self, 
        LLVMMetadataRef scope, 
        string name, 
        LLVMMetadataRef file, 
        uint lineNumber, 
        ulong sizeInBits, 
        uint alignInBits,
        LLVMMetadataRef[] elements,
        string uniqueId,
        LLVMDIFlags flags = DefaultFlags
        )
    {
        var mname = new MarshaledString(name);
        var melements = new MarshaledArray<LLVMMetadataRef, nint>(elements, m => m.Handle);
        var muniqueId = new MarshaledString(uniqueId);

        return new((nint)LLVM.DIBuilderCreateStructType
        (
            (LLVMOpaqueDIBuilder*)self.Handle,
            scope.ToPtr(), mname.Value, (nuint)mname.Length,
            file.ToPtr(), lineNumber, sizeInBits, alignInBits,
            flags, null, (LLVMOpaqueMetadata**)melements.Value, 
            (uint)melements.Length, 0, null, muniqueId.Value, (nuint)muniqueId.Value
        ));
    }
}