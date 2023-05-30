using Antelcat.Foundation.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Foundation.Core.Structs;

public class ServiceInfos
{
    public ServiceInfos(IServiceProvider serviceProvider,
        Func<Type, ImplementInfo> statCreator)
    {
        ServiceProvider = serviceProvider;
        StatCreator = statCreator;
    }

    public IServiceProvider ServiceProvider { get; }

    private Func<Type, ImplementInfo> StatCreator { get; }

    /// <summary>
    /// 缓存的服务生命周期
    /// </summary>
    public Dictionary<Type, ServiceLifetime>? ServiceLifetimes { get; set; }
    
    /// <summary>
    /// 缓存的实现类的属性字段映射器
    /// </summary>
    private Dictionary<Type, ImplementInfo> CachedMappers { get; init; }= new();
    
    /// <summary>
    /// 解析过的单例
    /// </summary>
    public HashSet<Type> ResolvedSingletons { get; private init; } = new();
    
    /// <summary>
    /// 解析过的会话
    /// </summary>
    public HashSet<Type> ResolvedScopes { get; } = new();

    /// <summary>
    /// 是否需要被解析
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool NoNeedAutowired(Type serviceType, object? target)
    {
        if (ResolvedSingletons.Contains(serviceType)
            || ResolvedScopes.Contains(serviceType)
            || target is null) return true;
        if (target is IServiceScopeFactory) return false;
        return !GetStat(target.GetType()).NeedAutowired;
    }

    public ImplementInfo GetStat(Type implementType)
    {
        if (CachedMappers.TryGetValue(implementType, out var r)) return r;
        r = StatCreator(implementType);
        CachedMappers.Add(implementType, r);
        return r;
    }
    
    /// <summary>
    /// 创建一个Scope
    /// </summary>
    /// <returns></returns>
    public ServiceInfos CreateScope()
    {
        return new ServiceInfos(ServiceProvider, StatCreator)
        {
            CachedMappers = CachedMappers,
            ResolvedSingletons = ResolvedSingletons,
            ServiceLifetimes = ServiceLifetimes
        };
    }
}

public struct ImplementInfo
{
    public bool NeedAutowired { get; init; }
    public List<Tuple<Type, Setter<object, object>>>? Mappers;
}