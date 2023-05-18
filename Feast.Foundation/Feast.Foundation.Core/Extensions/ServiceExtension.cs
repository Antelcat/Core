using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Feast.Foundation.Core.Attributes;
using Feast.Foundation.Core.Implements.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feast.Foundation.Core.Extensions
{
    public static class ServiceExtension
    {
        private static readonly Func<ServiceDescriptor, bool> Condition = descriptor
            => descriptor.Lifetime == ServiceLifetime.Singleton;

        public static IServiceCollection CopySingletons(this IServiceCollection collection,
            IServiceProvider services)
            => collection.CopySingletons(services,
                collection
                    .Where(x => Condition(x))
                    .Select(x => x.ServiceType));

        public static IServiceCollection CopySingletons(this IServiceCollection collection,
            IServiceProvider services,
            IServiceCollection fromCollection)
            => collection.CopySingletons(services,
                fromCollection
                    .Where(x => Condition(x))
                    .Select(x => x.ServiceType));

        public static IServiceCollection CopySingletons(this IServiceCollection collection,
            IServiceProvider services,
            IEnumerable<Type> serviceTypes)
            => serviceTypes.Aggregate(collection, (c, s) =>
            {
                try
                {
                    var instance = services.GetService(s);
                    if (instance != null) collection.Add(new ServiceDescriptor(s, instance));
                }catch { /**/ }
                return c;
            });

        public static IServiceCollection RedirectSingletons(this IServiceCollection collection,
            IServiceProvider services)
            => collection.RedirectSingletons(services, collection
                .Where(x => Condition(x))
                .Select(x => x.ServiceType));

        public static IServiceCollection RedirectSingletons(this IServiceCollection collection,
            IServiceProvider services,
            IServiceCollection fromCollection)
            => collection.RedirectSingletons(services, fromCollection
                .Where(x => Condition(x))
                .Select(x => x.ServiceType));

        public static IServiceCollection RedirectSingletons(this IServiceCollection collection,
            IServiceProvider services,
            IEnumerable<Type> serviceTypes)
            => serviceTypes.Aggregate(collection, (c, s) => c
                .AddSingleton(s, _ => services
                    .GetRequiredService(s)));

        public static IServiceProvider BuildAutowiredServiceProvider(this IServiceCollection collection,
            Func<IServiceCollection, IServiceProvider> builder)
            => new AutowiredServiceProvider(builder(collection));

        public static IServiceProvider BuildAutowiredServiceProvider<TAttribute>(this IServiceCollection collection,
            Func<IServiceCollection, IServiceProvider> builder) where TAttribute : Attribute
            => new AutowiredServiceProvider<TAttribute>(builder(collection));
    }
}
