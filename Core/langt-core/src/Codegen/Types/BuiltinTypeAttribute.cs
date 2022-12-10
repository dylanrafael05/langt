namespace Langt.Codegen;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
public sealed class BuiltinTypeAttribute : Attribute
{
    public BuiltinTypeAttribute() {}
}
