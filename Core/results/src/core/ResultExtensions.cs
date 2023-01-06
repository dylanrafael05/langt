using Results.Interfaces;

namespace Results;

public static class ResultExtensions
{
    public static R WithError<R>(this R r, IResultError err) where R : IModdable<R>
        => r.WithErrors(new IResultError[] {err});
    public static R WithMetadata<R>(this R r, IResultMetadata meta) where R : IModdable<R>
        => r.WithMetadata(new IResultMetadata[] {meta});

    public static R WithDataFrom<R>(this R r, IResultlike other) where R : IModdable<R>
        => r.WithErrors(other.Errors).WithMetadata(other.Metadata);

    public static R WithoutError<R>(this R r, IResultError err) where R : IModdable<R>, IResultlike
        => r.ClearErrors().WithErrors(r.Errors.Except(new[] {err}));
    public static R WithoutMetadata<R>(this R r, IResultMetadata err) where R : IModdable<R>, IResultlike
        => r.ClearMetadata().WithMetadata(r.Metadata.Except(new[] {err}));

    public static T Or<R, T>(this R r, T orValue) where R : IResultlike, IValued<T>
        => r.HasValue ? r.Value : orValue;
    public static T Or<R, T>(this R r, Func<T> orValueProvider) where R : IResultlike, IValued<T>
        => r.HasValue ? r.Value : orValueProvider();

    public static R Forgive<R, T>(this R r, Func<T> defaultValue) where R : IModdable<R>, IValueModdable<R, T>, IValued<T>, IResultlike
        => r.Forgive().WithValue(r.Or(defaultValue)!);
    public static R Forgive<R, T>(this R r, T defaultValue) where R : IModdable<R>, IValueModdable<R, T>, IValued<T>, IResultlike
        => r.Forgive(() => defaultValue);
    
    public static R Forgive<R>(this R r) where R : IModdable<R>, IResultlike
    {
        var nmeta = from e in r.Errors let t = e.TryDemote() where t is not null select t;
        return r
            .ClearErrors()
            .WithMetadata(nmeta)
        ;
    }

    public static bool AnyErr<T>(this IResultlike r) where T : IResultError
        => r.Errors.Any(e => e is T);
    public static bool AnyMeta<T>(this IResultlike r) where T : IResultMetadata
        => r.Metadata.Any(e => e is T);

    public static T GetSingleton<T>(this IResultlike r) where T : IResultMonoid<T>
        => r.Metadata.OfType<T>().FirstOrDefault(T.Identity);
    public static R ModifySingleton<T, R>(this R r, Func<T, T> modifier) 
        where T : IResultMonoid<T> 
        where R : IResultlike, IModdable<R>
    {
        var instances = r.Metadata.OfType<T>().ToArray();

        if(instances.Length == 0)
        {
            var newinst = modifier(T.Identity);
            return r.WithMetadata(newinst);
        }
        else 
        {
            var inst = instances[0];
            var newinst = modifier(inst);

            if(object.ReferenceEquals(inst, newinst))
            {
                return r;
            }

            return r.WithoutMetadata(inst).WithMetadata(newinst);
        }
    }

    // General Result //
    public static void IfErrors(this IResultlike r1, Action<IEnumerable<IResultError>> a) 
    {
        if(r1.HasErrors) a(r1.Errors);
    }
    public static void IfMetadata(this IResultlike r1, Action<IEnumerable<IResultMetadata>> a) 
    {
        if(r1.HasMetadata) a(r1.Metadata);
    }
    
    // Void Result //
    public static Result And(this Result r1, Result r2) 
        => r1.HasErrors ? r1 : r2.WithDataFrom(r2);
    public static Result GreedyAnd(this Result r1, Result r2) 
        => r1.WithDataFrom(r2);

    // Value Results //
    private static Result<T> MergeData<T>(Result<T> item, IResultlike a, IResultlike b)
        => item
            .WithDataFrom(a)
            .WithDataFrom(b);
    
    
    public static Result<(T1, T2)> And<T1, T2>(this Result<T1> r1, Result<T2> r2)
        => !r1 ? Result.Error<(T1, T2)>(r1.Errors) : r1.GreedyAnd(r2);
    public static Result<(T1, T2, T3)> And<T1, T2, T3>(this Result<(T1, T2)> r1, Result<T3> r2)
        => !r1 ? Result.Error<(T1, T2, T3)>(r1.Errors) : r1.GreedyAnd(r2);
    public static Result<(T1, T2, T3, T4)> And<T1, T2, T3, T4>(this Result<(T1, T2, T3)> r1, Result<T4> r2)
        => !r1 ? Result.Error<(T1, T2, T3, T4)>(r1.Errors) : r1.GreedyAnd(r2);
    public static Result<(T1, T2, T3, T4, T5)> And<T1, T2, T3, T4, T5>(this Result<(T1, T2, T3, T4)> r1, Result<T5> r2)
        => !r1 ? Result.Error<(T1, T2, T3, T4, T5)>(r1.Errors) : r1.GreedyAnd(r2);
    public static Result<(T1, T2, T3, T4, T5, T6)> And<T1, T2, T3, T4, T5, T6>(this Result<(T1, T2, T3, T4, T5)> r1, Result<T6> r2)
        => !r1 ? Result.Error<(T1, T2, T3, T4, T5, T6)>(r1.Errors) : r1.GreedyAnd(r2);
        
    public static Result<(T1, T2)> GreedyAnd<T1, T2>(this Result<T1> r1, Result<T2> r2)
        => MergeData(new Result<(T1, T2)>((r1.PossibleValue!, r2.PossibleValue!)), r1, r2);
    public static Result<(T1, T2, T3)> GreedyAnd<T1, T2, T3>(this Result<(T1, T2)> r1, Result<T3> r2)
        => MergeData(new Result<(T1, T2, T3)>((r1.PossibleValue.Item1, r1.PossibleValue.Item2, r2.PossibleValue!)), r1, r2);
    public static Result<(T1, T2, T3, T4)> GreedyAnd<T1, T2, T3, T4>(this Result<(T1, T2, T3)> r1, Result<T4> r2)
        => MergeData(new Result<(T1, T2, T3, T4)>((r1.PossibleValue.Item1, r1.PossibleValue.Item2, r1.PossibleValue.Item3, r2.PossibleValue!)), r1, r2);
    public static Result<(T1, T2, T3, T4, T5)> GreedyAnd<T1, T2, T3, T4, T5>(this Result<(T1, T2, T3, T4)> r1, Result<T5> r2)
        => MergeData(new Result<(T1, T2, T3, T4, T5)>((r1.PossibleValue.Item1, r1.PossibleValue.Item2, r1.PossibleValue.Item3, r1.PossibleValue.Item4, r2.PossibleValue!)), r1, r2);
    public static Result<(T1, T2, T3, T4, T5, T6)> GreedyAnd<T1, T2, T3, T4, T5, T6>(this Result<(T1, T2, T3, T4, T5)> r1, Result<T6> r2)
        => MergeData(new Result<(T1, T2, T3, T4, T5, T6)>((r1.PossibleValue.Item1, r1.PossibleValue.Item2, r1.PossibleValue.Item3, r1.PossibleValue.Item4, r1.PossibleValue.Item5, r2.PossibleValue!)), r1, r2);

    public static Result<(T1, T2)> Attach<T1, T2>(this Result<T1> r1, T2 v)
        => r1.HasErrors ? r1.Cast<(T1, T2)>() : Result.Success<(T1, T2)>((r1.Value, v)).WithDataFrom(r1);
    public static Result<(T1, T2, T3)> Attach<T1, T2, T3>(this Result<(T1, T2)> r1, T3 v)
        => r1.HasErrors ? r1.Cast<(T1, T2, T3)>() : Result.Success<(T1, T2, T3)>((r1.Value.Item1, r1.Value.Item2, v)).WithDataFrom(r1);
    public static Result<(T1, T2, T3, T4)> Attach<T1, T2, T3, T4>(this Result<(T1, T2, T3)> r1, T4 v)
        => r1.HasErrors ? r1.Cast<(T1, T2, T3, T4)>() : Result.Success<(T1, T2, T3, T4)>((r1.Value.Item1, r1.Value.Item2, r1.Value.Item3, v)).WithDataFrom(r1);
}