namespace Results;

public struct Result : IResult<Result>
{
    bool IResult.HasValue => !HasErrors;

    public bool HasErrors {get; private init;} = false;
    public bool HasMetadata {get; private init;} = false;
    
    object? IResult.Value => ((IResult)this).HasValue 
        ? null 
        : throw new InvalidOperationException($"Cannot get .{nameof(IResult.Value)} if .{nameof(IResult.HasValue)} returns false!")
        ;

    public IEnumerable<IResultError> Errors {get; init;}
    public IEnumerable<IResultMetadata> Metadata {get; init;}

    public Result WithError(IResultError error) => new(Errors.Append(error), true, Metadata, HasMetadata);
    public Result WithErrors(IResultError first, params IResultError[] rest) => WithError(first).WithErrors(rest);
    public Result WithErrors(IEnumerable<IResultError> errors) => new(Errors.Concat(errors), HasErrors || errors.Any(), Metadata, HasMetadata);
    public Result WithMetadata(IResultMetadata metadata) => new(Errors, HasErrors, Metadata.Append(metadata), true);
    public Result WithMetadata(IResultMetadata first, params IResultMetadata[] rest) => WithMetadata(first).WithMetadata(rest);
    public Result WithMetadata(IEnumerable<IResultMetadata> metadata) => new(Errors, HasErrors, Metadata.Concat(metadata), HasMetadata || metadata.Any());

    public Result WithDataFrom(IResult other)
        => WithErrors(other.Errors)
          .WithMetadata(other.Metadata);
    public Result WithDataFrom(ResultBuilder builder)
        => WithErrors(builder.Errors)
          .WithMetadata(builder.Metadata);

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
        => ((IResult)self).HasValue;
    public static bool operator false(Result self) 
        => ((IResult)self).HasErrors;

    public Result() : this(Array.Empty<IResultError>(), false, Array.Empty<IResultMetadata>(), false)
    {}
    private Result(IEnumerable<IResultError> errors, bool err, IEnumerable<IResultMetadata> metadata, bool meta)
    {
        Errors = errors;
        HasErrors = err;

        Metadata = metadata;
        HasMetadata = meta;
    }

    public static Result Success() => new();
    public static Result Error(IEnumerable<IResultError> errors) => Success().WithErrors(errors);
    public static Result Error(IResultError first, params IResultError[] rest) => Error(Enumerable.Empty<IResultError>().Append(first).Concat(rest));
    
    public static Result<T> Success<T>(T value) => new(value);
    public static Result<T> Error<T>(IEnumerable<IResultError> errors) => Success<T>(default!).WithErrors(errors);
    public static Result<T> Error<T>(IResultError first, params IResultError[] rest) => Error<T>(Enumerable.Empty<IResultError>().Append(first).Concat(rest));

    public static Result Wrap(IResult r) => new(r.Errors, r.HasErrors, r.Metadata, r.HasMetadata);

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

    /// <summary>
    /// Applies a Result-returning function to every item in an array and returns the 
    /// resulting array of objects, stopping early and returning an Error if
    /// any of the function calls produce one.
    /// </summary>
    /// <param name="input">The array to manipulate</param>
    /// <param name="resultor">The function to apply; can return a success or error</param>
    /// <typeparam name="TIn">The input type</typeparam>
    /// <typeparam name="TOut">The output type</typeparam>
    /// <returns>
    /// A result which tells if any results during array manipulation were errors.
    /// Its encapsulated array is not guaranteed to be the same size as the input array
    /// if any errors occured.
    /// </returns>
    public static Result<TOut[]> Foreach<TIn, TOut>(IEnumerable<TIn> input, Func<TIn, Result<TOut>> resultor)
    {
        var builder = ResultBuilder.Empty();
        var result = new List<TOut>();

        foreach(var v in input)
        {
            var r = resultor(v);
            builder.AddData(r);

            if(!r) break;

            result.Add(r.Value);
        }

        return builder.Build(result.ToArray());
    }
    public static Result Foreach<TIn>(IEnumerable<TIn> input, Func<TIn, Result> resultor)
        => All(input.Select(resultor));

    /// <summary>
    /// Applies a Result-returning function to every item in an array and returns the 
    /// resulting array of objects, accumulating Errors as they occur.
    /// </summary>
    /// <param name="input">The array to manipulate</param>
    /// <param name="resultor">The function to apply; can return a success or error</param>
    /// <typeparam name="TIn">The input type</typeparam>
    /// <typeparam name="TOut">The output type</typeparam>
    /// <returns>
    /// A result which tells if any results during array manipulation were errors. 
    /// Its encapsulated array is guaranteed to be the same size as the input array always,
    /// but will contain invalid or null instances if an error occured.
    /// </returns>
    public static Result<TOut[]> GreedyForeach<TIn, TOut>(IEnumerable<TIn> input, Func<TIn, Result<TOut>> resultor)
    {
        var builder = ResultBuilder.Empty();
        var result = new List<TOut>();

        foreach(var v in input)
        {
            var r = resultor(v);

            result.Add(r.OrDefault()!);
            builder.AddData(r);
        }

        return builder.Build(result.ToArray());
    }
    public static Result GreedyForeach<TIn>(IEnumerable<TIn> input, Func<TIn, Result> resultor)
        => GreedyAll(input.Select(resultor));

    public static Result<TOut[]> ForgiveForeach<TIn, TOut>(IEnumerable<TIn> input, Func<TIn, Result<TOut>> resultor, TOut defaultValue)
    {
        var builder = ResultBuilder.Empty();
        var result = new List<TOut>();

        foreach(var v in input)
        {
            var r = resultor(v).Forgive(defaultValue);

            result.Add(r.Value);
            builder.AddData(r);
        }

        return builder.Build(result.ToArray());
    }
    public static Result ForgiveForeach<TIn>(IEnumerable<TIn> input, Func<TIn, Result> resultor)
        => ForgiveAll(input.Select(resultor));

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
