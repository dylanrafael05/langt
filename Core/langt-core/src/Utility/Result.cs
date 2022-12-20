using System.Diagnostics.CodeAnalysis;

namespace Langt.Utility;

public class ResultException : Exception
{
    public object Error {get; init;}
    public ResultException(object err)  {Error = err;}
}

public sealed class Resultor
{
    private Resultor() {}

    public static Resultor Success => new();
    public static Resultor Failure(object err)
    {
        Result.Throw(err);
        return null!;
    }
}

public sealed class Resultor<T>
{
    private T r;

    public T Value => r;

    private Resultor(T r) {this.r = r;}

    public static implicit operator Resultor<T>(T r) => new(r);
    public static implicit operator T(Resultor<T> r) => r.r;

    public static Resultor<T> Success(T r) => r;
    public static Resultor<T> Failure(object err)
    {
        Result.Throw(err);
        return null!;
    }
}

public interface IResult
{
    bool HasError {get;}
    object Error {get;}
}

public sealed class Result<T> : IResult
{
    private readonly object? obj;
    private readonly bool isObj;

    public override string? ToString() => HasValue ? Value?.ToString() : $"Error: '" + Error + "'";

    public T Value => !isObj ? throw new InvalidOperationException("Cannot get the value of an error result") : (T)obj!;
    public bool HasValue => isObj;
    [MemberNotNullWhen(true, nameof(HasError))]
    public object Error => isObj ? "none" : obj!;
    public bool HasError => !isObj;

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Result<T> r && r.obj == this.obj;
    public override int GetHashCode()
        => obj!.GetHashCode();

    public static bool operator ==(Result<T> a, Result<T> b)
        => a.Equals(b);
    public static bool operator !=(Result<T> a, Result<T> b)
        => !(a == b);

    public static bool operator true(Result<T> t) => t.HasValue; 
    public static bool operator false(Result<T> t) => t.HasError;
    public static explicit operator T(Result<T> a) => a.Value;

    internal Result(T obj)
    {
        this.obj = obj;
        this.isObj = true;
    }
    internal Result(object error)
    {
        this.obj = error;
        this.isObj = false;
    }
}

/// <summary>
/// Represents a non-value: equivalent in theory to the "void" type.
/// </summary>
public struct Unit 
{
    public static readonly Unit Value = default;
}

public sealed class Result : IResult
{
    public static void Throw(object Error)
        => throw new ResultException(Error);

    public static Result Try(Func<Resultor> f)
    {
        try
        {
            f();
            return new();
        }
        catch(ResultException r)
        {
            return new(r.Error);
        }
    }
    public static Result Try<T1>(Func<T1,Resultor> f, T1 t1)
        => Try(()=>f(t1));
    public static Result Try<T1,T2>(Func<T1,T2,Resultor> f, T1 t1, T2 t2)
        => Try(()=>f(t1,t2));
    public static Result Try<T1,T2,T3>(Func<T1,T2,T3,Resultor> f, T1 t1, T2 t2, T3 t3)
        => Try(()=>f(t1,t2,t3));
    public static Result Try<T1,T2,T3,T4>(Func<T1,T2,T3,T4,Resultor> f, T1 t1, T2 t2, T3 t3, T4 t4)
        => Try(()=>f(t1,t2,t3,t4));
    public static Result Try<T1,T2,T3,T4,T5>(Func<T1,T2,T3,T4,T5,Resultor> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        => Try(()=>f(t1,t2,t3,t4,t5));
    public static Result Try<T1,T2,T3,T4,T5,T6>(Func<T1,T2,T3,T4,T5,T6,Resultor> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        => Try(()=>f(t1,t2,t3,t4,t5,t6));
    public static Result Try<T1,T2,T3,T4,T5,T6,T7>(Func<T1,T2,T3,T4,T5,T6,T7,Resultor> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        => Try(()=>f(t1,t2,t3,t4,t5,t6,t7));
    public static Result Try<T1,T2,T3,T4,T5,T6,T7,T8>(Func<T1,T2,T3,T4,T5,T6,T7,T8,Resultor> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        => Try(()=>f(t1,t2,t3,t4,t5,t6,t7,t8));

    public static Result<TRet> Try<TRet>(Func<Resultor<TRet>> f)
    {
        try
        {
            return new Result<TRet>(f());
        }
        catch(ResultException r)
        {
            return new Result<TRet>(r.Error);
        }
    }
    public static Result<TRet> Try<TRet,T1>(Func<T1,Resultor<TRet>> f, T1 t1)
        => Try(()=>f(t1));
    public static Result<TRet> Try<TRet,T1,T2>(Func<T1,T2,Resultor<TRet>> f, T1 t1, T2 t2)
        => Try(()=>f(t1,t2));
    public static Result<TRet> Try<TRet,T1,T2,T3>(Func<T1,T2,T3,Resultor<TRet>> f, T1 t1, T2 t2, T3 t3)
        => Try(()=>f(t1,t2,t3));
    public static Result<TRet> Try<TRet,T1,T2,T3,T4>(Func<T1,T2,T3,T4,Resultor<TRet>> f, T1 t1, T2 t2, T3 t3, T4 t4)
        => Try(()=>f(t1,t2,t3,t4));
    public static Result<TRet> Try<TRet,T1,T2,T3,T4,T5>(Func<T1,T2,T3,T4,T5,Resultor<TRet>> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        => Try(()=>f(t1,t2,t3,t4,t5));
    public static Result<TRet> Try<TRet,T1,T2,T3,T4,T5,T6>(Func<T1,T2,T3,T4,T5,T6,Resultor<TRet>> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        => Try(()=>f(t1,t2,t3,t4,t5,t6));
    public static Result<TRet> Try<TRet,T1,T2,T3,T4,T5,T6,T7>(Func<T1,T2,T3,T4,T5,T6,T7,Resultor<TRet>> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        => Try(()=>f(t1,t2,t3,t4,t5,t6,t7));
    public static Result<TRet> Try<TRet,T1,T2,T3,T4,T5,T6,T7,T8>(Func<T1,T2,T3,T4,T5,T6,T7,T8,Resultor<TRet>> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        => Try(()=>f(t1,t2,t3,t4,t5,t6,t7,t8));

    public bool HasError {get;}
    public object Error {get;}
    
    public override string? ToString() => HasError ? $"Error: '" + Error + "'" : "Success";
    
    public static bool operator true(Result t) => !t.HasError; 
    public static bool operator false(Result t) => t.HasError;

    private Result()
    {
        HasError = false;
        Error = null!;
    }
    private Result(object err) 
    {
        HasError = true;
        Error = err;
    }
}