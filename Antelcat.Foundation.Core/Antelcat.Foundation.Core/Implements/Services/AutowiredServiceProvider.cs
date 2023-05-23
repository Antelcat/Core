using System.Reflection;
using System.Runtime.Serialization;
using Antelcat.Foundation.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Foundation.Core.Implements.Services;

public abstract class BaseAutowiredServiceProvider<TAttribute> : IServiceProvider, ISupportRequiredService
    where TAttribute : Attribute
{
    private readonly IServiceProvider serviceProvider;

    protected BaseAutowiredServiceProvider(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;
    public object? GetService(Type serviceType) => Autowired(serviceProvider.GetService(serviceType));

    public object GetRequiredService(Type serviceType) => GetService(serviceType) ?? throw new SerializationException($"Unable to resolve service : [ {serviceType} ]");
    
    protected BindingFlags GetFlags(ref object? instance)
    {
        var flags = BindingFlags.Public | BindingFlags.NonPublic;
        if (instance is Type)
        {
            instance = null;
            flags |= BindingFlags.Static;
        }
        else
        {
            flags |= BindingFlags.Instance;
        }

        return flags;
    }
    
    protected IEnumerable<FieldInfo> GetAutowiredFields(Type type, BindingFlags flags) => type.GetFields(flags)
        .Where(x => x.GetCustomAttribute<TAttribute>() != null);

    protected IEnumerable<PropertyInfo> GetAutowiredProps(Type type, BindingFlags flags) => type.GetProperties(flags)
        .Where(x => x.GetCustomAttribute<TAttribute>() != null);

    protected abstract object? Autowired(object? target);
}

public class AutowiredServiceProvider<TAttribute> : BaseAutowiredServiceProvider<TAttribute>
    where TAttribute : Attribute
{
    public AutowiredServiceProvider(IServiceProvider serviceProvider) :base(serviceProvider){}

    protected override object? Autowired(object? target)
    {
        if (target == null) return target;
        var type = target as Type ?? target.GetType();

        var flags = GetFlags(ref target);

        foreach (var field in GetAutowiredFields(type, flags))
        {
            var dependency = GetService(field.FieldType);
            if (dependency != null) field.SetValue(target, dependency);
        }

        foreach (var property in GetAutowiredProps(type, flags))
        {
            var dependency = GetService(property.PropertyType);
            if (dependency != null) property.SetValue(target, dependency);
        }

        return target;
    }
}


public class CachedAutowiredServiceProvider<TAttribute> : BaseAutowiredServiceProvider<TAttribute>
    where TAttribute : Attribute
{
    public CachedAutowiredServiceProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }

    private readonly Dictionary<Type, List<Tuple<Type, Setter<object, object>>>> mapperCache = new();
    protected override object? Autowired(object? target)
    {
        if (target == null) return target;
        var type = target as Type ?? target.GetType();

        if (!mapperCache.TryGetValue(type, out var mapper))
        {
            var flags = GetFlags(ref target);

            mapper = new List<Tuple<Type, Setter<object, object>>>();
            mapper.AddRange(GetAutowiredFields(type,flags)
                .Select(x => new Tuple<Type, Setter<object, object>>(x.FieldType, x.CreateSetter())));
            mapper.AddRange(GetAutowiredProps(type, flags)
                .Select(x => new Tuple<Type, Setter<object, object>>(x.PropertyType, x.CreateSetter())));
            mapperCache.Add(type, mapper);
        }

        mapper.ForEach(x =>
        {
            var dependency = GetService(x.Item1);
            if (dependency != null) x.Item2.Invoke(ref target, dependency);
        });

        return target;
    }
}