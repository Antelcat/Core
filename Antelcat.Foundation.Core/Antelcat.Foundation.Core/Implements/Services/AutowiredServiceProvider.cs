using System.Reflection;
using System.Runtime.Serialization;
using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Core.Structs;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Foundation.Core.Implements.Services;

public abstract class ProxiedServiceProvider<TAttribute> 
    : IServiceProvider, ISupportRequiredService
    where TAttribute : Attribute
{
    protected readonly IServiceProvider ServiceProvider;
    
    protected ProxiedServiceProvider(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;
    
    public abstract object? GetService(Type serviceType);

    public object GetRequiredService(Type serviceType) =>
        GetService(serviceType) 
        ?? throw new SerializationException($"Unable to resolve service : [ {serviceType} ]");

    private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    protected IEnumerable<Tuple<Type, Setter<object, object>>> GetSetter(Type implementType)
    {
        return implementType
            .GetProperties(Flags)
            .Where(static x => x.CanRead && x.GetCustomAttribute<TAttribute>() != null)
            .Select(static x => new Tuple<Type, Setter<object, object>>(x.PropertyType, x.CreateSetter()))
            .Concat(implementType
                .GetFields(Flags)
                .Where(static x => x.GetCustomAttribute<TAttribute>() != null)
                .Select(static x => new Tuple<Type, Setter<object, object>>(x.FieldType, x.CreateSetter())
                ));
    }
}

public abstract class CachedAutowiredServiceProvider<TAttribute> 
    : ProxiedServiceProvider<TAttribute>
    where TAttribute : Attribute
{
    protected CachedAutowiredServiceProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }
    protected ServiceStats SharedStats { get; init; } = new();
    protected void Autowired(object target, Func<Type, object?> depProvider)
    {
        var type = target.GetType();
        if (!SharedStats.CachedMappers.TryGetValue(type, out var mapper))
        {
            mapper = GetSetter(type).ToList();
            SharedStats.CachedMappers.Add(type, mapper);
        }

        mapper.ForEach(x =>
        {
            var dependency = depProvider(x.Item1);
            if (dependency != null) x.Item2.Invoke(ref target!, dependency);
        });
    }
}

public class TransientAutowiredServiceProvider<TAttribute> 
    : CachedAutowiredServiceProvider<TAttribute>
    where TAttribute : Attribute
{
    public TransientAutowiredServiceProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public override object? GetService(Type serviceType)
    {
        var impl = ServiceProvider.GetService(serviceType);
        if (impl == null) return impl;
        Autowired(impl, s => ServiceProvider.GetService(s));
        return impl;
    }
}


public class AutowiredServiceProvider<TAttribute> : CachedAutowiredServiceProvider<TAttribute>
    where TAttribute : Attribute
{
    private readonly Dictionary<Type,ServiceLifetime> serviceLifetimes;
    private AutowiredServiceProvider(IServiceProvider serviceProvider,
        Dictionary<Type, ServiceLifetime> serviceLifetimes) : base(serviceProvider) =>
        this.serviceLifetimes = serviceLifetimes;

    private bool ValidLifetime(
        ServiceLifetime currentLifetime,
        Type targetType,
        out ServiceLifetime targetLifetime) =>
        serviceLifetimes.TryGetValue(targetType, out targetLifetime)
            ? currentLifetime switch
            {
                ServiceLifetime.Singleton => targetLifetime is ServiceLifetime.Singleton,
                ServiceLifetime.Scoped => targetLifetime is not ServiceLifetime.Transient,
                ServiceLifetime.Transient => true,
                _ => throw new ArgumentOutOfRangeException(
                    $"{currentLifetime} is not presented in {nameof(ServiceLifetime)}")
            }
            : throw new NullReferenceException(
                $"Lifetime of {targetType} is not presented in {nameof(ServiceCollection)}");

    public AutowiredServiceProvider(IServiceProvider serviceProvider, IServiceCollection collection)
        : this(serviceProvider, collection.ToDictionary(
            static x => x.ServiceType,
            static x => x.Lifetime)) { }

    public override object? GetService(Type serviceType)
    {
        var target = ServiceProvider.GetService(serviceType);
        if (SharedStats.IsResolved(serviceType)) return target;
        return target switch
        {
            null => null,
            IServiceScopeFactory factory => new AutowiredServiceScopeFactory(factory,
                s => new AutowiredServiceProvider<TAttribute>(s, serviceLifetimes)
                    { SharedStats = SharedStats.CreateScope() }),
            _ => GetServiceInternal(target, serviceType)
        };
    }

    private object? GetServiceInternal(object target, Type serviceType) =>
        serviceLifetimes.TryGetValue(serviceType, out var lifetime)
            ? GetServiceInternal(target, serviceType, lifetime)
            : throw new SerializationException("Service lifetime uncertain");

    private object? GetServiceDependency(object? target, Type serviceType, ServiceLifetime lifetime) =>
        target == null
            ? null
            : SharedStats.IsResolved(serviceType)
                ? target
                : GetServiceInternal(target, serviceType, lifetime);

    private object? GetServiceInternal(object target, Type serviceType, ServiceLifetime lifetime)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                SharedStats.ResolvedSingletons.Add(serviceType);
                break;
            case ServiceLifetime.Scoped:
                SharedStats.ResolvedScopes.Add(serviceType);
                break;
        }

        Autowired(target, targetType => ValidLifetime(lifetime, targetType, out var targetLifetime)
            ? GetServiceDependency(ServiceProvider.GetService(targetType), targetType, targetLifetime)
            : throw new NotSupportedException(
                $"Type {serviceType} by lifetime {lifetime} can not have {targetType} of lifetime {targetLifetime}"));
        return target;
    }
  
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