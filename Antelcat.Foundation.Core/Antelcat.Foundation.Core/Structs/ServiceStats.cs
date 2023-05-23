using Antelcat.Foundation.Core.Extensions;

namespace Antelcat.Foundation.Core.Structs;

public class ServiceStats
{
    public Dictionary<Type, List<Tuple<Type, Setter<object, object>>>> CachedMappers { get; private init; }= new();
    public List<Type> ResolvedSingletons { get; private init; } = new();
    public List<Type> ResolvedScopes { get; } = new();

    public bool IsResolved(Type type) => ResolvedScopes.Contains(type) || ResolvedSingletons.Contains(type);
    public ServiceStats CreateScope()
    {
        return new ServiceStats
        {
            CachedMappers = CachedMappers,
            ResolvedSingletons = ResolvedSingletons
        };
    }
}