using System.Runtime.CompilerServices;

namespace Langt.Structure;

public static class ResolvableExtensions
{
    public static bool ResolutionEqual(this IResolutionlike self, IResolutionlike other)
        => self.Name == other.Name && ReferenceEquals(self.HoldingScope, other.HoldingScope);

    public static string FullNameSimple(IScope? holdingScope, string displayName)
    {
        if(holdingScope is null) return displayName;

        var u = holdingScope;

        while(u.Parent is not IResolvable and not null)
        {
            u = u.Parent;
        }

        if(u is IResolvable r) return r.FullName + "::" + displayName;

        return displayName;
    }
    public static string FullNameSimple(this IResolvable self) 
        => FullNameSimple(self.HoldingScope, self.DisplayName);
    
    public static bool Incomplete(this CompletionState state) 
        => state < CompletionState.Complete;

    public static void AssertIncomplete(this IResolvable self, [CallerMemberName] string callerName = "!")
    {
        Expect.That(self.Completion.Incomplete(), $"Resolution must be incomplete to call .{callerName}");
    }
    
    public static IFullNamed UnwrapProxy(this IResolvable self) => self switch 
    {
        ProxyResolution proxy => proxy.Item,
        _                     => self
    };

    public static Result<IFullNamed> UnwrapProxy(this Result<IResolvable> self)
        => self.Map(UnwrapProxy);
}


