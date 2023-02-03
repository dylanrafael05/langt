namespace Langt.Utility;

public static class TypeExtensions
{
    public static string ReadableName(this Type ty) => ty switch 
    {
        {Name: nameof(Byte)}    => "byte",
        {Name: nameof(SByte)}   => "sbyte",
        {Name: nameof(Int16)}   => "short",
        {Name: nameof(UInt16)}  => "ushort",
        {Name: nameof(Int32)}   => "int",
        {Name: nameof(UInt32)}  => "uint",
        {Name: nameof(Int64)}   => "long",
        {Name: nameof(UInt64)}  => "ulong",
        {Name: nameof(IntPtr)}  => "nint",
        {Name: nameof(UIntPtr)} => "nuint",
        {Name: nameof(Char)}    => "char",
        {Name: nameof(Single)}  => "float",
        {Name: nameof(Double)}  => "double",
        {Name: nameof(Decimal)} => "decimal",
        {Name: nameof(Boolean)} => "bool",
        {Name: nameof(String)}  => "string",
        {Name: nameof(Object)}  => "object",
        {Name: "Void"}          => "void",

        {IsArray: true} => ty.GetElementType()!.ReadableName() + "[" + ",".Repeat(ty.GetArrayRank()-1) + "]",

        {IsConstructedGenericType: true} => ty.Name.TakeWhile(t => t is not '`').ReString() + "<" + ty.GenericTypeArguments.Select(ReadableName).Stringify() + ">",

        _ => ty.Name
    };
}