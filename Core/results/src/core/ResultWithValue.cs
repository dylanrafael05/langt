namespace Results;

public struct Result<T> : IPrimitiveResult<Result<T>>
{
    private T? valueStorage;

    public bool HasValue {get; private init;} = false;
    public bool HasErrors {get; private init;} = false;
    public bool HasMetadata {get; private init;} = false;

    object? IResult.Value => this.Value;

    public T Value 
    {
        get => HasValue
            ? valueStorage!
            : throw new InvalidOperationException($"Cannot get .{nameof(IResult.Value)} if .{nameof(IResult.HasValue)} returns false!")
            ;
        
        init => valueStorage = value;
    }
    public IEnumerable<IResultError> Errors {get; init;}
    public IEnumerable<IResultMetadata> Metadata {get; init;}

    public T? PossibleValue => HasValue
        ? valueStorage
        : default
        ;

    public static explicit operator Result<T>(T i) => Result.Success(i);

    public static bool operator !(Result<T> self)
        => self.HasErrors;
    
    public static bool operator true(Result<T> self) 
        => ((IResult)self).HasValue;
    public static bool operator false(Result<T> self) 
        => ((IResult)self).HasErrors;

    public Result<T> WithError(IResultError error) => new(Value, HasValue, Errors.Append(error), true, Metadata, HasMetadata);
    public Result<T> WithErrors(IResultError first, params IResultError[] rest) => WithError(first).WithErrors(rest);
    public Result<T> WithErrors(IEnumerable<IResultError> errors) => new(Value, HasValue, Errors.Concat(errors), HasErrors || errors.Any(), Metadata, HasMetadata);
    public Result<T> WithMetadata(IResultMetadata metadata) => new(Value, HasValue, Errors, HasErrors, Metadata.Append(metadata), true);
    public Result<T> WithMetadata(IResultMetadata first, params IResultMetadata[] rest) => WithMetadata(first).WithMetadata(rest);
    public Result<T> WithMetadata(IEnumerable<IResultMetadata> metadata) => new(Value, HasValue, Errors, HasErrors, Metadata.Concat(metadata), HasMetadata || metadata.Any());

    public Result<T> WithDataFrom(IResult other)
        => WithErrors(other.Errors)
          .WithMetadata(other.Metadata);
    public Result<T> WithDataFrom(ResultBuilder other)
        => WithErrors(other.Errors)
          .WithMetadata(other.Metadata);
    
    public Result<T> Forgive(T defaultValue)
    {
        var nmeta = from e in Errors let t = e.TryDemote() where t is not null select t;
        return Result.Success(Or(defaultValue)!).WithMetadata(Metadata).WithMetadata(nmeta);
    }
    public Result<T> Forgive(Func<T> defaultValueProvider)
    {
        var nmeta = from e in Errors let t = e.TryDemote() where t is not null select t;
        return Result.Success(OrFrom(defaultValueProvider)!).WithMetadata(Metadata).WithMetadata(nmeta);
    }

    public Result<TOther> Cast<TOther>()
    {
        if(!HasErrors) throw new InvalidOperationException($".{nameof(Cast)} received a non-error result");

        return Result.Blank<TOther>().WithDataFrom(this);
    }

    public Result Drop()
        => Result.Blank().WithDataFrom(this);

    public T Expect(string reason = "expected valid result", Func<string, Exception>? exceptionBuilder = null)
    {
        if(!HasValue)
        {
            throw exceptionBuilder?.Invoke(reason) ?? new InvalidOperationException($".{nameof(Expect)} called on an invalid result: {reason}");
        }

        return Value;
    }

    public Result() : this(default, false, Array.Empty<IResultError>(), false, Array.Empty<IResultMetadata>(), false)
    {}
    public Result(T value) : this(value, true, Array.Empty<IResultError>(), false, Array.Empty<IResultMetadata>(), false)
    {}
    private Result(T? value, bool val, IEnumerable<IResultError> errors, bool err, IEnumerable<IResultMetadata> metadata, bool meta)
    {
        valueStorage = value;
        HasValue = val && !err; // ensure that errors dominate values

        Errors = errors;
        HasErrors = err;

        Metadata = metadata;
        HasMetadata = meta;
    }

    public Result<T> And(Result other) 
        => WithDataFrom(other);

    public T? OrDefault() 
    {
        if(HasErrors) return default;
        return Value;
    }

    public T? Or(T? def) 
    {
        if(HasErrors) return def;
        return Value;
    }
    public T? OrFrom(Func<T?> def) 
    {
        if(HasErrors) return def();
        return Value;
    }
    public T? OrFrom(Func<Result<T>, T?> def) 
    {
        if(HasErrors) return def(this);
        return Value;
    }

    public void Match(Action<T> success, Action<IEnumerable<IResultError>> error) 
    {
        if(HasValue) success(Value);
        else         error(Errors);
    }
    public R Match<R>(Func<T,R> success, Func<IEnumerable<IResultError>,R> error) 
    {
        if(HasValue) return success(Value);
        else         return error(Errors);
    }

    public Result<TOut> Map<TOut>(Func<T,TOut> mapper)
    {
        if(HasValue) return Result.Success(mapper(Value)).WithDataFrom(this);
        else         return Result.Error<TOut>(Errors).WithDataFrom(this);
    }

    public Result<TOut> Combine<TOther, TOut>(Result<TOther> other, Func<T, TOther, TOut> combinator)
    {
        if(HasValue && other.HasValue) return Result.Success(combinator(Value, other.Value)).WithDataFrom(this).WithDataFrom(other);
        else                           return Result.Error<TOut>(Errors).WithMetadata(Metadata).WithDataFrom(other);
    }
    public Result<TOut> Combine<TOther, TOut>(Result<TOther> other, Func<T, TOther, Result<TOut>> combinator)
    {
        if(HasValue && other.HasValue) return combinator(Value, other.Value).WithDataFrom(this).WithDataFrom(other);
        else                           return Result.Error<TOut>(Errors).WithMetadata(Metadata).WithDataFrom(other);
    }
}
