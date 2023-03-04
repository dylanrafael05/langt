namespace Langt.Structure;

public interface IScope
{
    IEnumerable<IResolvable> Items(Context ctx);
    IScope? Parent {get;}
    Result<IResolvable> ResolveDirect(string identifier, SourceRange range, Context ctx, bool upwards = true);
    Result<IFullNamed> Resolve(string identifier, SourceRange range, Context ctx, bool upwards = true);
    Result Define(IResolvable value, SourceRange range); 
}


