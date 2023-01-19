using System.Runtime.CompilerServices;

namespace Langt.Utility;

public static class Expect
{
    #region Arguments
    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgNonNull(object? arg, string? message = null, [CallerArgumentExpression(nameof(arg))] string argname = "ERROR")
    {
        if(arg is null)
        {
            throw new ArgumentNullException(argname, message);
        }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgHasContent(string? arg, string? message = null, [CallerArgumentExpression(nameof(arg))] string argname = "ERROR")
    {
        if(arg is null or "")
        {
            throw new ArgumentException(message ?? $"Argument expected not to be null or whitespace", argname);
        }
    }
    #endregion
    #region Inline
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Is<T>(object? arg, string? message = null, [CallerArgumentExpression(nameof(arg))] string argname = "ERROR")
    {
        if(arg is not T t) 
        {
            #if DEBUG
                throw new Exception(message ?? $"Value [{argname}] must be an instance of {typeof(T).FullName}");
            #else
                return default(T);
            #endif
        }

        return t;
    }

    public static object NonNull(object? arg, string? message = null, [CallerArgumentExpression(nameof(arg))] string argname = "ERROR")
    {
        #if DEBUG
        if(arg is null) 
        {
            throw new Exception(message ?? $"Value [{argname}] was expected to be non-null");
        }
        #endif

        return arg!;
    }
    #endregion
    #region Assertion-like
    [Conditional("DEBUG")]
    public static void That(bool arg, string? message = null, [CallerArgumentExpression(nameof(arg))] string argname = "ERROR")
    {
        if(arg) 
        {
            throw new Exception(message ?? $"Value [{argname}] was expected to be true but was not");
        }
    }

    [Conditional("DEBUG")]
    public static void Not(bool arg, string? message = null, [CallerArgumentExpression(nameof(arg))] string argname = "ERROR")
    {
        if(!arg) 
        {
            throw new Exception(message ?? $"Value [{argname}] was expected to be false but was not");
        }
    }
    #endregion
}