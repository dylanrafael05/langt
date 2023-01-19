namespace Results.Interfaces;

public interface IResultOptions<Self> : IResultMetadata where Self: IResultOptions<Self>
{
    Self Merge(Self other); 
}