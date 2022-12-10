namespace Langt.Utility;

public static class ULongFormat
{
    public static unsafe ulong I64(long x) 
    {
        return *(ulong*)&x;
    }
    public static unsafe ulong I32(int x) 
    {
        return *(uint*)&x;
    }
    public static unsafe ulong I16(short x) 
    {
        return *(ushort*)&x;
    }
    public static unsafe ulong I8 (sbyte x) 
    {
        return *(byte*)&x;
    }
}