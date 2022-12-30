namespace Langt.Codegen;

public static class ScopedImpl
{
    public static string GetFullName(this IScoped? s) 
    {
        if(s is null) return "";

        var name = (s as INamed)?.DisplayName;

        if(s.HoldingScope is null) return name ?? "";

        var upperName = GetFullName(s.HoldingScope!);
        if(string.IsNullOrEmpty(upperName)) return name ?? "";

        return upperName + (name is null ? "" : "::" + name);
    }
}  