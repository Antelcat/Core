using System.Reflection;
using System.Runtime.Serialization;
using Antelcat.Foundation.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Foundation.Core.Implements.Services;

public class AutowiredServiceProvider<TAttribute> 
    : IServiceProvider, ISupportRequiredService
    where TAttribute : Attribute
{
    private readonly IServiceProvider serviceProvider;
    public AutowiredServiceProvider(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;
    public object GetRequiredService(Type serviceType) => 
        GetService(serviceType) ?? throw new SerializationException($"Unable to resolve service : [ {serviceType} ]");

    public object? GetService(Type serviceType) => Autowried(serviceProvider.GetService(serviceType));
    private object? Autowried(object? target)
    {
        if (target == null) return target;
        var flags = BindingFlags.Public | BindingFlags.NonPublic;
        var type = target as Type ?? target.GetType();
        if (target is Type)
        {
            target = null;
            flags |= BindingFlags.Static;
        }
        else
        {
            flags |= BindingFlags.Instance;
        }

        foreach (var field in type.GetFields(flags))
        {
            var attr = field.GetCustomAttribute<TAttribute>();
            if (attr == null) continue;
            var dependency = GetService(field.FieldType);
            if (dependency != null) field.SetValue(target, dependency);
        }

        foreach (var property in type.GetProperties(flags).Where(x=>x.CanWrite))
        {
            var attr = property.GetCustomAttribute<TAttribute>();
            if (attr == null) continue;
            var dependency = GetService(property.PropertyType);
            if (dependency != null) property.SetValue(target, dependency);
        }
        return target;
    }
}

public class AutowiredServiceProviderFactory<TAttribute>
    : IServiceProviderFactory<IServiceCollection>
    where TAttribute : Attribute
{
    private readonly Func<IServiceCollection, IServiceProvider> builder;

    public AutowiredServiceProviderFactory(Func<IServiceCollection, IServiceProvider> builder) =>
        this.builder = builder;
    public IServiceCollection CreateBuilder(IServiceCollection? services) =>
        services ?? new ServiceCollection();
    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) =>
        new AutowiredServiceProvider<TAttribute>(builder(containerBuilder));
}

/// <summary>
/// 专门用来解析 <see cref="AutowiredAttribute"/> 的自动注解生成器
/// </summary>
public sealed class AutowiredServiceProvider : AutowiredServiceProvider<AutowiredAttribute>
{
    public AutowiredServiceProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }
}

/// <summary>
/// 专门用来解析 <see cref="AutowiredAttribute"/> 的服务工厂
/// </summary>
public sealed class AutowiredServiceProviderFactory : AutowiredServiceProviderFactory<AutowiredAttribute>
{
    public AutowiredServiceProviderFactory(Func<IServiceCollection, IServiceProvider> builder) : base(builder) { }
}