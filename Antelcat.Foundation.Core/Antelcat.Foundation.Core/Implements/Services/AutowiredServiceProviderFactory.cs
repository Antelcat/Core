using Microsoft.Extensions.DependencyInjection;

namespace Antelcat.Foundation.Core.Implements.Services;

public abstract class BaseServiceProviderFactory<TServiceProvider> : IServiceProviderFactory<IServiceCollection> 
where TServiceProvider : IServiceProvider
{
    protected abstract TServiceProvider ProvideService(IServiceCollection collection);
    
    public IServiceCollection CreateBuilder(IServiceCollection? services) => services ?? new ServiceCollection();

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => ProvideService(containerBuilder);
}

public class AutowiredServiceProviderFactory<TAttribute> : BaseServiceProviderFactory<AutowiredServiceProvider<TAttribute>>
    where TAttribute : Attribute
{
    private readonly Func<IServiceCollection, IServiceProvider> builder;

    public AutowiredServiceProviderFactory(Func<IServiceCollection, IServiceProvider> builder) => this.builder = builder;

    protected override AutowiredServiceProvider<TAttribute> ProvideService(IServiceCollection collection) => new(builder(collection));
}


public class CachedAutowiredServiceProviderFactory<TAttribute> : BaseServiceProviderFactory<CachedAutowiredServiceProvider<TAttribute>>
    where TAttribute : Attribute
{
    private readonly Func<IServiceCollection, IServiceProvider> builder;
    
    public CachedAutowiredServiceProviderFactory(Func<IServiceCollection, IServiceProvider> builder) => this.builder = builder;

    protected override CachedAutowiredServiceProvider<TAttribute> ProvideService(IServiceCollection collection) => new(builder(collection));
}