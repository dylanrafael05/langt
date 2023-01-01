namespace Results;

public struct ResultGroup : IResultlike<ResultGroup>
{
    
    public IEnumerable<IResultError> Errors {get; init;}
    public IEnumerable<IResultMetadata> Metadata {get; init;}

    public bool HasErrors {get; private init;} = false;
    public bool HasMetadata {get; private init;} = false;

    public IEnumerable<Result> InnerResults {get; init;}
    
    public ResultGroup(IEnumerable<Result> innerResults)
    {
        Errors = innerResults.SelectMany(r => r.Errors).ToArray();
        HasErrors = innerResults.Any(r => r.HasErrors);
        
        Metadata = innerResults.SelectMany(r => r.Metadata).ToArray();
        HasMetadata = innerResults.Any(r => r.HasMetadata);

        InnerResults = innerResults;
    }

    public Result Combine() => Result.Blank().WithDataFrom(this);

    public static bool operator !(ResultGroup self)
        => self.HasErrors;
    public static bool operator true(ResultGroup self)
        => !(!self);
    public static bool operator false(ResultGroup self)
        => !self;
        
    public static ResultGroup From(params Result[] results) => new(results);
    public static ResultGroup From(IEnumerable<Result> results) => new(results);
    public static ResultGroup<T> From<T>(params Result<T>[] results) => new(results);
    public static ResultGroup<T> From<T>(IEnumerable<Result<T>> results) => new(results);

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
    public static ResultGroup<TOut> Foreach<TIn, TOut>(IEnumerable<TIn> input, Func<TIn, Result<TOut>> resultor)
    {
        var result = new List<Result<TOut>>();

        foreach(var v in input)
        {
            var r = resultor(v);
            result.Add(r);

            if(!r) break;
        }

        return From(result);
    }
    public static ResultGroup Foreach<TIn>(IEnumerable<TIn> input, Func<TIn, Result> resultor)
    {
        var result = new List<Result>();

        foreach(var v in input)
        {
            var r = resultor(v);
            result.Add(r);

            if(!r) break;
        }

        return From(result);
    }

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
    public static ResultGroup<TOut> GreedyForeach<TIn, TOut>(IEnumerable<TIn> input, Func<TIn, Result<TOut>> resultor)
    {
        var result = new List<Result<TOut>>();

        foreach(var v in input)
        {
            var r = resultor(v);

            result.Add(r);
        }

        return From(result);
    }
    public static ResultGroup GreedyForeach<TIn>(IEnumerable<TIn> input, Func<TIn, Result> resultor)
        => From(input.Select(resultor).ToArray());
}