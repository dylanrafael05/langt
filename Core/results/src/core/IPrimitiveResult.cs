namespace Results;

public interface IPrimitiveResult<Self> : IResult<Self> where Self : IPrimitiveResult<Self>
{
    Self WithError(IResultError error);
    Self WithErrors(IResultError first, params IResultError[] rest);
    Self WithErrors(IEnumerable<IResultError> errors);
    Self WithMetadata(IResultMetadata metadata);
    Self WithMetadata(IResultMetadata first, params IResultMetadata[] rest);
    Self WithMetadata(IEnumerable<IResultMetadata> metadata);

    Self WithDataFrom(IResult other);
    Self WithDataFrom(ResultBuilder builder);
}