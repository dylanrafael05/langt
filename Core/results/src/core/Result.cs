namespace Results;

public struct Result : IMutableResultlike<Result>
{
    public bool HasErrors {get; private init;} = false;
    public bool HasMetadata {get; private init;} = false;

    public IEnumerable<IResultError> Errors {get; init;}
    public IEnumerable<IResultMetadata> Metadata {get; init;}

    public Result WithErrors(IEnumerable<IResultError> errors) => new(Errors.Concat(errors).ToArray(), HasErrors || errors.Any(), Metadata, HasMetadata);
    public Result WithMetadata(IEnumerable<IResultMetadata> metadata) => new(Errors, HasErrors, Metadata.Concat(metadata).ToArray(), HasMetadata || metadata.Any());
    public Result ExcludingErrors(IEnumerable<IResultError> errors)
    {
        var nerr = Errors.Except(errors).ToArray();
        return new(nerr, nerr.Length > 0, Metadata, HasMetadata);
    }
    public Result ExcludingMetadata(IEnumerable<IResultMetadata> metadata)
    {
        var nmd = Metadata.Except(metadata).ToArray();
        return new(Errors, HasErrors, nmd, nmd.Length > 0);
    }

    public Result Forgive()
    {
        var nmeta = from e in Errors let t = e.TryDemote() where t is not null select t;
        return Success().WithMetadata(Metadata).WithMetadata(nmeta);
    }

    public void Expect(string reason = "expected valid result", Func<string, Exception>? exceptionBuilder = null)
    {
        if(HasErrors)
        {
            throw exceptionBuilder?.Invoke(reason) ?? new InvalidOperationException($".{nameof(Expect)} called on an invalid result: {reason}");
        }
    }


    public static bool operator !(Result self)
        => self.HasErrors;
    
    public static bool operator true(Result self) 
        => !!self;
    public static bool operator false(Result self) 
        => !self;

    public Result() : this(Array.Empty<IResultError>(), false, Array.Empty<IResultMetadata>(), false)
    {}
    private Result(IEnumerable<IResultError> errors, bool err, IEnumerable<IResultMetadata> metadata, bool meta)
    {
        Errors = errors;
        HasErrors = err;

        Metadata = metadata;
        HasMetadata = meta;
    }

    public static Result Blank() => new();
    public static Result Success() => new();
    public static Result Error(IEnumerable<IResultError> errors) => Success().WithErrors(errors);
    public static Result Error(IResultError first, params IResultError[] rest) => Error(Enumerable.Empty<IResultError>().Append(first).Concat(rest));
    
    public static Result<T> Blank<T>() => new();
    public static Result<T> Success<T>(T value) => new(value);
    public static Result<T> Error<T>(IEnumerable<IResultError> errors) => Success<T>(default!).WithErrors(errors);
    public static Result<T> Error<T>(IResultError first, params IResultError[] rest) => Error<T>(Enumerable.Empty<IResultError>().Append(first).Concat(rest));

    public static Result Wrap(IResultlike r) => new(r.Errors, r.HasErrors, r.Metadata, r.HasMetadata);
    public static Result<T> Wrap<T>(T value, IResultlike r) => Success(value).WithDataFrom(r);

    public static Result<(T1, T2)> GreedyAll<T1, T2>(Result<T1> a, Result<T2> b)
        => a.GreedyAnd(b);
    public static Result<(T1, T2, T3)> GreedyAll<T1, T2, T3>(Result<T1> a, Result<T2> b, Result<T3> c)
        => a.GreedyAnd(b).GreedyAnd(c);
    public static Result<(T1, T2, T3, T4)> GreedyAll<T1, T2, T3, T4>(Result<T1> a, Result<T2> b, Result<T3> c, Result<T4> d)
        => a.GreedyAnd(b).GreedyAnd(c).GreedyAnd(d);
    public static Result<(T1, T2, T3, T4, T5)> GreedyAll<T1, T2, T3, T4, T5>(Result<T1> a, Result<T2> b, Result<T3> c, Result<T4> d, Result<T5> e)
        => a.GreedyAnd(b).GreedyAnd(c).GreedyAnd(d).GreedyAnd(e);
    public static Result<(T1, T2, T3, T4, T5, T6)> GreedyAll<T1, T2, T3, T4, T5, T6>(Result<T1> a, Result<T2> b, Result<T3> c, Result<T4> d, Result<T5> e, Result<T6> f)
        => a.GreedyAnd(b).GreedyAnd(c).GreedyAnd(d).GreedyAnd(e).GreedyAnd(f);

    public static Result<(T1, T2)> All<T1, T2>(Result<T1> a, Result<T2> b)
        => a.And(b);
    public static Result<(T1, T2, T3)> All<T1, T2, T3>(Result<T1> a, Result<T2> b, Result<T3> c)
        => a.And(b).And(c);
    public static Result<(T1, T2, T3, T4)> All<T1, T2, T3, T4>(Result<T1> a, Result<T2> b, Result<T3> c, Result<T4> d)
        => a.And(b).And(c).And(d);
    public static Result<(T1, T2, T3, T4, T5)> All<T1, T2, T3, T4, T5>(Result<T1> a, Result<T2> b, Result<T3> c, Result<T4> d, Result<T5> e)
        => a.And(b).And(c).And(d).And(e);
    public static Result<(T1, T2, T3, T4, T5, T6)> All<T1, T2, T3, T4, T5, T6>(Result<T1> a, Result<T2> b, Result<T3> c, Result<T4> d, Result<T5> e, Result<T6> f)
        => a.And(b).And(c).And(d).And(e).And(f);

    public static Result All(IEnumerable<Result> results)
        => results.Aggregate((r, n) => r.And(n));
    public static Result All(params Result[] results)
        => All(results);
        
    public static Result GreedyAll(IEnumerable<Result> results)
        => results.Aggregate((r, n) => r.GreedyAnd(n));
    public static Result GreedyAll(params Result[] results)
        => All(results);

    public static Result ForgiveAll(IEnumerable<Result> results)
        => results.Aggregate((r, n) => r.Forgive().And(n)).Forgive();
    public static Result ForgiveAll(params Result[] results)
        => ForgiveAll(results);

    public static Result SkipAll(IEnumerable<Result> results)
        => results.Aggregate((r, n) => n ? r.GreedyAnd(n) : r);
    public static Result SkipAll(params Result[] results)
        => SkipAll(results);

    public static Result<T> Try<T>(Func<T> resultor)
    {
        try { return Success(resultor()); }
        catch(Exception e) 
        { return Error<T>(new ExceptionError(e)); }
    }
    public static Result<T> Try<T>(Func<T> resultor, Predicate<Exception> exceptionPredicate)
    {
        try { return Success(resultor()); }
        catch(Exception e) 
        { 
            if(exceptionPredicate(e)) return Error<T>(new ExceptionError(e)); 
            throw;
        }
    }
}
