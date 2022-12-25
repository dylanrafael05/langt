namespace Results;

public class ResultBuilder 
{
    public static ResultBuilder Empty() => new();
    public static ResultBuilder FromData(IResult result) => Empty().WithData(result);

    private readonly List<IResultError> errors = new();
    private readonly List<IResultMetadata> metadata = new();

    public IReadOnlyList<IResultError> Errors => errors;
    public bool HasErrors => errors.Count != 0;
    public IReadOnlyList<IResultMetadata> Metadata => metadata;
    public bool HasMetadata => metadata.Count != 0;

    public void AddError(IResultError err) => errors.Add(err);
    public void AddErrors(params IResultError[] errs) => errors.AddRange(errs);
    public void AddErrors(IEnumerable<IResultError> errs) => errors.AddRange(errs);
    public void AddMetadata(IResultMetadata meta) => metadata.Add(meta);
    public void AddMetadata(params IResultMetadata[] meta) => metadata.AddRange(meta);
    public void AddMetadata(IEnumerable<IResultMetadata> meta) => metadata.AddRange(meta);
    public void AddData(IResult result) 
    {
        AddErrors(result.Errors);
        AddMetadata(result.Metadata);
    }

    public ResultBuilder WithError(IResultError error) 
    {
        AddError(error);
        return this;
    }
    public ResultBuilder WithErrors(params IResultError[] errors) 
    {
        AddErrors(errors);
        return this;
    }
    public ResultBuilder WithErrors(IEnumerable<IResultError> errors) 
    {
        AddErrors(errors);
        return this;
    }
    public ResultBuilder WithMetadata(IResultMetadata meta) 
    {
        AddMetadata(meta);
        return this;
    }
    public ResultBuilder WithMetadata(params IResultMetadata[] meta) 
    {
        AddMetadata(meta);
        return this;
    }
    public ResultBuilder WithMetadata(IEnumerable<IResultMetadata> meta) 
    {
        AddMetadata(meta);
        return this;
    }
    public ResultBuilder WithData(in IResult result)
    {
        AddData(result);
        return this;
    }

    
    public static bool operator !(ResultBuilder self)
        => self.HasErrors;
    
    public static bool operator true(ResultBuilder self) 
        => !self.HasErrors;
    public static bool operator false(ResultBuilder self) 
        => self.HasErrors;

    public Result Build() => Result.Success().WithDataFrom(this);
    public Result<T> Build<T>()
    {
        if(!HasErrors)
        {
            throw new InvalidOperationException($"Attempted to build a valued-result without a value using a {GetType().Name} without any errors present");
        }

        return Result.Success<T>(default!).WithDataFrom(this);
    }
    public Result<T> Build<T>(T value) => Result.Success(value).WithDataFrom(this);
}
