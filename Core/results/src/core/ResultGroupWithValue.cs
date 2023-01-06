namespace Results;
using Interfaces;

public struct ResultGroup<T> : IResultlike, IValued<IEnumerable<T>>, IResultOperators<ResultGroup<T>>
{
    bool IValued<IEnumerable<T>>.HasValue => !HasErrors;
    public IEnumerable<T> Value => ((IValued<IEnumerable<T>>)this).HasValue 
        ? InnerResults.Select(r => r.Value) 
        : throw new InvalidOperationException($"Cannot get .{nameof(IValued<IEnumerable<T>>.Value)} if .{nameof(IValued<IEnumerable<T>>.HasValue)} returns false!")
        ;

    public IEnumerable<IResultError> Errors {get; init;}
    public IEnumerable<IResultMetadata> Metadata {get; init;}

    public bool HasErrors {get; private init;} = false;
    public bool HasMetadata {get; private init;} = false;

    public IEnumerable<T?> WithDefault(T? defaultValue = default)
        => InnerResults.Select(r => r.HasValue ? r.Value : defaultValue);
    public IEnumerable<T?> WithDefault(Func<T?> defaultValueProducer)
        => InnerResults.Select(r => r.HasValue ? r.Value : defaultValueProducer());
    public IEnumerable<T> SkipErrors()
        => InnerResults.Where(r => r.HasValue).Select(r => r.Value);

    public IEnumerable<IValuedResultlike<T>> InnerResults {get; init;}
    
    public Result<IEnumerable<T>> Combine() => Result.Success<IEnumerable<T>>(WithDefault()!).WithDataFrom(this);
    public Result<IEnumerable<T?>> CombineWithDefault(T? defaultValue = default) => Result.Success(WithDefault(defaultValue)).WithDataFrom(this);
    public Result<IEnumerable<T?>> CombineWithDefault(Func<T?> defaultValueProducer) => Result.Success(WithDefault(defaultValueProducer)).WithDataFrom(this);
    public Result<IEnumerable<T>> CombineSkip() => Result.Success(SkipErrors()).WithMetadata(Metadata);
    
    public ResultGroup(IEnumerable<IValuedResultlike<T>> innerResults)
    {
        Errors = innerResults.SelectMany(r => r.Errors);
        HasErrors = innerResults.Any(r => r.HasErrors);
        
        Metadata = innerResults.SelectMany(r => r.Metadata);
        HasMetadata = innerResults.Any(r => r.HasMetadata);

        InnerResults = innerResults;
    }

    public static bool operator !(ResultGroup<T> self)
        => self.HasErrors;
    public static bool operator true(ResultGroup<T> self)
        => !(!self);
    public static bool operator false(ResultGroup<T> self)
        => !self;
}
