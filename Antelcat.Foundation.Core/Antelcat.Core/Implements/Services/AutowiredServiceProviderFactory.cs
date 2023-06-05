using Antelcat.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Core.Implements.Services;

public abstract class BaseServiceProviderFactory<TServiceProvider> 
    : IServiceProviderFactory<IServiceCollection> 
where TServiceProvider : IServiceProvider
{
    protected abstract TServiceProvider ProvideService(IServiceCollection collection);

    public IServiceCollection CreateBuilder(IServiceCollection? services) => services ?? new ServiceCollection();

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => 
        ProvideService(containerBuilder);
}

public class AutowiredServiceProviderFactory<TAttribute> 
    : BaseServiceProviderFactory<AutowiredServiceProvider<TAttribute>>
    where TAttribute : Attribute
{
    private readonly Func<IServiceCollection, IServiceProvider> builder;

    public AutowiredServiceProviderFactory(Func<IServiceCollection, IServiceProvider> builder) =>
        this.builder = builder;

    protected override AutowiredServiceProvider<TAttribute> ProvideService(IServiceCollection collection) =>
        new(builder(collection), collection);
}


public class AutowiredServiceScopeFactory : IServiceScopeFactory
{
    private readonly IServiceScopeFactory proxy;
    private readonly Func<IServiceProvider, IServiceProvider> provider;
    public AutowiredServiceScopeFactory(IServiceScopeFactory factory,
        Func<IServiceProvider,IServiceProvider> provider)
    {
        proxy = factory;
        this.provider = provider;
    }

    public IServiceScope CreateScope() => new AutowiredServiceScope(proxy.CreateScope(), provider);
}

