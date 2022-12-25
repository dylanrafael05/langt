namespace Results;

public static class ResultExtensions
{
    // General Result //
    public static void IfErrors(this IResult r1, Action<IEnumerable<IResultMetadata>> a) 
    {
        if(r1.HasErrors) a(r1.Errors);
    }
    public static void IfMetadata(this IResult r1, Action<IEnumerable<IResultMetadata>> a) 
    {
        if(r1.HasInfo) a(r1.Info);
    }
    
    // Void Result //
    public static Result And(this Result r1, Result r2) 
        => r1.HasErrors ? r1 : r2.WithDataFrom(r2);
    public static Result GreedyAnd(this Result r1, Result r2) 
        => r1.WithDataFrom(r2);

    // Value Results //
    private static Result<T> MergeData<T>(Result<T> item, IResult a, IResult b)
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
}