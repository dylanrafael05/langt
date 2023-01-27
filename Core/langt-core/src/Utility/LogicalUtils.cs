namespace Langt.Utility;

public static class LogicalUtils
{
    public static bool OrFalse(this bool? b) => b ?? false;
    public static bool OrTrue (this bool? b) => b ?? true;
}