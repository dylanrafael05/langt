namespace Results;
using Interfaces;

public struct Result<T> : IModdableResultlike<Result<T>>, IValuedResultlike<T>, IResultOperators<Result<T>>
{
    private T? valueStorage;

    public bool HasValue {get; private init;} = false;
    public bool HasErrors {get; private init;} = false;
    public bool HasMetadata {get; private init;} = false;

    public T Value 
    {
        get => HasValue
            ? valueStorage!
            : throw new InvalidOperationException($"Cannot get .{nameof(IValued<T>.Value)} if .{nameof(IValued<T>.HasValue)} returns false!")
            ;
        
        init => valueStorage = value;
    }
    public IEnumerable<IResultError> Errors {get; init;}
    public IEnumerable<IResultMetadata> Metadata {get; init;}

    public T? PossibleValue => HasValue
        ? valueStorage
        : default
        ;

    public static bool operator !(Result<T> self)
        => self.HasErrors;
    
    public static bool operator true(Result<T> self) 
        => self.HasValue;
    public static bool operator false(Result<T> self) 
        => self.HasErrors;

    public Result<T> WithErrors(IEnumerable<IResultError> errors) => new(PossibleValue, HasValue, Errors.Concat(errors).ToArray(), HasErrors || errors.Any(), Metadata, HasMetadata);
    public Result<T> WithMetadata(IEnumerable<IResultMetadata> metadata) => new(PossibleValue, HasValue, Errors, HasErrors, IResultMetadata.MergeMetadataImmediate(metadata, Metadata), HasMetadata || metadata.Any());
    public Result<T> ClearErrors() => new(PossibleValue, HasValue, Array.Empty<IResultError>(), false, Metadata, HasMetadata);
    public Result<T> ClearMetadata() => new(PossibleValue, HasValue, Errors, HasErrors, Array.Empty<IResultMetadata>(), false);

    public Result<T> WithValue(T value) => new(value, true, Errors, HasErrors, Metadata, HasMetadata);
    public Result<T> ExcludingValue() => new(default, false, Errors, HasErrors, Metadata, HasMetadata);
    
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

    public Result<TOther> ErrorCast<TOther>()
    {
        if(!HasErrors) throw new InvalidOperationException($".{nameof(ErrorCast)} received a non-error result");

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
        => this.WithDataFrom(other);

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
        else         return Result.Blank<TOut>().WithDataFrom(this);
    }
    public Result<TOut> Map<TOut>(Func<T,Result<TOut>> mapper)
    {
        if(HasValue) return mapper(Value).WithDataFrom(this);
        else return Result.Blank<TOut>().WithDataFrom(this);
    }

    public Result<TOut> As<TOut>()
    {
        if(HasValue) return Result.Success((TOut)(object)Value!).WithDataFrom(this);
        else         return ErrorCast<TOut>();
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
