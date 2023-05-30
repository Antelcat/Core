using System.Reflection;
using System.Runtime.Serialization;
using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Core.Structs;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Foundation.Core.Implements.Services;

using SetterCache = Tuple<Type, Setter<object, object>>;

public abstract class ProxiedServiceProvider
    : IServiceProvider, ISupportRequiredService
{
    public object GetRequiredService(Type serviceType) =>
        GetService(serviceType)
        ?? throw new SerializationException($"Unable to resolve service : [ {serviceType} ]");

    protected void Autowired(object target, IEnumerable<SetterCache> mapper) =>
        mapper.ForEach(x =>
        {
            var dependency = ProvideDependency(x.Item1);
            if (dependency != null) x.Item2.Invoke(ref target!, dependency);
        });

    public abstract object? GetService(Type serviceType);

    protected abstract object? ProvideDependency(Type dependencyType);
}

public abstract class CachedAutowiredServiceProvider<TAttribute>
    : ProxiedServiceProvider
    where TAttribute : Attribute
{

    protected  IServiceProvider ServiceProvider => SharedInfos.ServiceProvider;

    #region Caches

    /// <summary>
    /// 共享的缓存数据
    /// </summary>
    protected ServiceInfos SharedInfos { get; init; }

    private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    /// <summary>
    /// 判断实现类型是否需要自动注入,如果需要则将Mapper添加到缓存中
    /// </summary>
    /// <param name="implementType"></param>
    /// <returns></returns>
    protected bool NeedAutowired(Type implementType) => SharedInfos.GetStat(implementType).NeedAutowired;

    private static ImplementInfo CreateStat(IReflect implementType)
    {
        var props = GetProps(implementType).ToList();
        var fields = GetFields(implementType).ToList();
        var need = props.Any() || fields.Any();
        var stat = new ImplementInfo
        {
            NeedAutowired = need
        };
        if (!need) return stat;
        stat.Mappers = props
            .Select(static x => new SetterCache(x.PropertyType, x.CreateSetter()))
            .Concat(fields.Select(static x => new SetterCache(x.FieldType, x.CreateSetter())))
            .ToList();
        return stat;
    }
    private static IEnumerable<PropertyInfo> GetProps(IReflect implementType) => implementType
        .GetProperties(Flags)
        .Where(static x => x.CanWrite && x.GetCustomAttribute<TAttribute>() != null);

    private static IEnumerable<FieldInfo> GetFields(IReflect implementType) => implementType
        .GetFields(Flags)
        .Where(static x => x.GetCustomAttribute<TAttribute>() != null);

    #endregion

    protected CachedAutowiredServiceProvider(IServiceProvider serviceProvider) =>
        SharedInfos = new ServiceInfos(serviceProvider, CreateStat);

    protected void Autowired(object target)
    {
        var type = target.GetType();
        var mapper = SharedInfos.GetStat(type);
        if (!mapper.NeedAutowired) return;
        Autowired(target, mapper.Mappers!);
    }
}

/// <summary>
/// 针对 <see cref="ServiceLifetime.Transient"/> 生命周期的依赖构造的Provider, 不需要考虑连续依赖的问题
/// </summary>
/// <typeparam name="TAttribute"></typeparam>
public class TransientAutowiredServiceProvider<TAttribute>
    : CachedAutowiredServiceProvider<TAttribute>
    where TAttribute : Attribute
{
    public TransientAutowiredServiceProvider(IServiceProvider serviceProvider) 
        : base(serviceProvider) { }

    public override object? GetService(Type serviceType)
    {
        var impl = ServiceProvider.GetService(serviceType);
        if (impl == null || !NeedAutowired(impl.GetType())) return impl;
        Autowired(impl);
        return impl;
    }

    protected override object? ProvideDependency(Type dependencyType) =>
        ServiceProvider.GetService(dependencyType);
}

public class AutowiredServiceProvider<TAttribute> 
    : CachedAutowiredServiceProvider<TAttribute>
    where TAttribute : Attribute
{
    private AutowiredServiceProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }

    private AutowiredServiceProvider(IServiceProvider serviceProvider,
        Dictionary<Type, ServiceLifetime> serviceLifetimes) : base(serviceProvider) =>
        SharedInfos.ServiceLifetimes = serviceLifetimes;

    public AutowiredServiceProvider(IServiceProvider serviceProvider, IServiceCollection collection)
        : this(serviceProvider, collection
            .Aggregate(new Dictionary<Type, ServiceLifetime>(), (d, s) =>
            {
                d.TryAdd(s.ServiceType, s.Lifetime);
                return d;
            }))
    {
    }

    public override object? GetService(Type serviceType)
    {
        var impl = ServiceProvider.GetService(serviceType);
        if (SharedInfos.NoNeedAutowired(serviceType, impl)) return impl;
        return impl switch
        {
            IServiceScopeFactory factory => new AutowiredServiceScopeFactory(factory,
                s => new AutowiredServiceProvider<TAttribute>(s)
                    { SharedInfos = SharedInfos.CreateScope() }),
            IEnumerable<object> collections => GetServicesInternal(collections, serviceType),
            _ => GetServiceInternal(impl!, serviceType)
        };
    }

    private object GetServicesInternal(IEnumerable<object> targets, Type serviceType)
    {
        var types = serviceType.GenericTypeArguments;
        if (types.Length == 0) throw new ArgumentException($"Service type {serviceType} has no generic type");
        var type = types[0];
        if (!TryGetServiceLifetime(type,  out var lifetime))
        {
            throw new SerializationException($"Service {serviceType} lifetime uncertain");
        }
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                SharedInfos.ResolvedSingletons.Add(serviceType);
                break;
            case ServiceLifetime.Scoped:
                SharedInfos.ResolvedScopes.Add(serviceType);
                break;
        }

        targets.ForEach(Autowired);
        return targets;
    }

    private object? GetServiceInternal(object instance, Type serviceType) =>
        TryGetServiceLifetime(serviceType, out var lifetime)
            ? AutowiredService(instance, serviceType, lifetime)
            : throw new SerializationException($"Service {serviceType} lifetime uncertain");


    private object? GetServiceDependency(object? target, Type serviceType, ServiceLifetime lifetime) =>
        SharedInfos.NoNeedAutowired(serviceType, target)
            ? target
            : AutowiredService(target!, serviceType, lifetime);

    private object AutowiredService(object target, Type serviceType, ServiceLifetime lifetime)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                SharedInfos.ResolvedSingletons.Add(serviceType);
                break;
            case ServiceLifetime.Scoped:
                SharedInfos.ResolvedScopes.Add(serviceType);
                break;
        }

        Autowired(target);
        return target;
    }

    protected override object? ProvideDependency(Type dependencyType)
    {
        var dep = ServiceProvider.GetService(dependencyType);
        return dep switch
        {
            null => null,
            IEnumerable<object> collection => GetServicesInternal(collection, dependencyType),
            _ => TryGetServiceLifetime(dependencyType,  out var targetLifetime)
                ? GetServiceDependency(dep, dependencyType, targetLifetime)
                : throw new SerializationException($"Dependency {dependencyType} lifetime uncertain")
        };
    }

    private bool TryGetServiceLifetime(Type serviceType,  out ServiceLifetime serviceLifetime) =>
        SharedInfos.ServiceLifetimes!.TryGetValue(serviceType, out serviceLifetime)
        || serviceType.IsGenericType
        && SharedInfos.ServiceLifetimes!.TryGetValue(serviceType.GetGenericTypeDefinition(),
            out serviceLifetime);
}

public class AutowiredServiceScope : IServiceScope
{
    private readonly IServiceScope proxy;

    public AutowiredServiceScope(IServiceScope scope, Func<IServiceProvider, IServiceProvider> provider)
    {
        proxy = scope;
        ServiceProvider = provider(scope.ServiceProvider);
    }

    public void Dispose() => proxy.Dispose();

    public IServiceProvider ServiceProvider { get; }
}