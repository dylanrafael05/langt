namespace Langt.Structure;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class BuiltinConversionAttribute : Attribute
{
    public BuiltinConversionAttribute() {}
}