namespace Results;

public class ResultBuilder : IMutableResultlike<ResultBuilder>
{
    public static ResultBuilder Empty() => new();
    public static ResultBuilder From(IResultlike first, params IResultlike[] rest) => Empty().WithData(first).WithData(rest);

    private readonly List<IResultError> errors = new();
    private readonly List<IResultMetadata> metadata = new();

    public IEnumerable<IResultError> Errors => errors;
    public bool HasErrors => errors.Count != 0;
    public IEnumerable<IResultMetadata> Metadata => metadata;
    public bool HasMetadata => metadata.Count != 0;

    public void AddError(IResultError err) => errors.Add(err);
    public void AddErrors(params IResultError[] errs) => errors.AddRange(errs);
    public void AddErrors(IEnumerable<IResultError> errs) => errors.AddRange(errs);
    public void AddMetadata(IResultMetadata meta) => metadata.Add(meta);
    public void AddMetadata(params IResultMetadata[] meta) => metadata.AddRange(meta);
    public void AddMetadata(IEnumerable<IResultMetadata> meta) => metadata.AddRange(meta);
    public void AddData(IResultlike result) 
    {
        AddErrors(result.Errors);
        AddMetadata(result.Metadata);
    }
    public void AddData(IEnumerable<IResultlike> results) 
    {
        foreach(var r in results)
        {
            AddErrors(r.Errors);
            AddMetadata(r.Metadata);
        }
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
    public ResultBuilder WithData(IResultlike result)
    {
        AddData(result);
        return this;
    }
    public ResultBuilder WithData(IEnumerable<IResultlike> result)
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

    public Result Build() => Result.Blank().WithDataFrom(this);
    public Result<T> Build<T>()
    {
        if(!HasErrors)
        {
            throw new InvalidOperationException($"Attempted to build a valued-result without a value using a {GetType().Name} without any errors present");
        }

        return Result.Blank<T>().WithDataFrom(this);
    }
    public Result<T> Build<T>(T value) => Result.Success(value).WithDataFrom(this);

    public void ExcludeErrors(IEnumerable<IResultError> errors)
    {
        var eset = errors.ToHashSet();
        this.errors.RemoveAll(p => eset.Contains(p));
    }
    public void ExcludeMetadata(IEnumerable<IResultMetadata> metadata)
    {
        var eset = metadata.ToHashSet();
        this.metadata.RemoveAll(p => eset.Contains(p));
    }

    public ResultBuilder ExcludingErrors(IEnumerable<IResultError> errors)
    {
        ExcludeErrors(errors);
        return this;
    }
    public ResultBuilder ExcludingMetadata(IEnumerable<IResultMetadata> metadata)
    {
        ExcludeMetadata(metadata);
        return this;
    }
}
