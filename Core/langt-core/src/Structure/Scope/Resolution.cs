// using Langt.AST;

// namespace Langt.Structure.Resolutions;

// // TODO: continue generalization of resolutions to remove previous interfaces
// public abstract class Resolution : IResolution
// {   
//     public abstract string Name {get;}
//     public virtual string DisplayName => Name;
//     public virtual string FullName => INamed.GetFullNameDefault(this);

//     public IScope HoldingScope {get;}
    
//     public SourceRange? DefinitionRange {get; init;}
//     public string? Documentation {get; init;}

//     public Resolution(IScope holdingScope) 
//     {
//         HoldingScope = holdingScope;
//     }
// }

// public class WeakRes<T> : IWeakRes where T : INamed
// {
//     public delegate Result<T> ProducerFunc(Context ctx, TypeCheckOptions options); 

//     public WeakRes(string name, IScope holding, ProducerFunc producer)
//     {
//         Name = name;
//         HoldingScope = holding;

//         fn = producer;
//     }

//     private readonly ProducerFunc fn;
//     private Result<T>? value;

//     public IScope HoldingScope {get;}
//     public string Name {get;}

//     public Result<T> Get(Context ctx, TypeCheckOptions? maybeOptions = null)
//         => value ??= fn(ctx, maybeOptions ?? new());
    
//     Result<INamed> IWeakRes.Get(Context ctx, TypeCheckOptions? optionsMaybe)
//         => Get(ctx, optionsMaybe).As<INamed>();
// }

// public interface IWeakRes 
// {
//     Result<INamed> Get(Context ctx, TypeCheckOptions? optionsMaybe = null);

//     IScope HoldingScope {get;}
//     string Name {get;}

//     public static WeakRes<T> Of<T>(T val, IScope scope) where T : INamed
//         => new(val.Name, scope, (_, _) => Result.Success(val));
//     public static WeakRes<T> Of<T>(T val) where T : IResolution
//         => new(val.Name, val.HoldingScope, (_, _) => Result.Success(val));
//     public static WeakRes<T> Of<T>(string name, IScope scope, WeakRes<T>.ProducerFunc producer) where T : INamed
//         => new(name, scope, producer);
// }