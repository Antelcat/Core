using System.Diagnostics;
using System.Runtime.InteropServices;
using Antelcat.Foundation.Core.Attributes;
using Antelcat.Foundation.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Feast.Foundation.Test
{
    public class Tests
    {
        readonly IServiceCollection collection = new ServiceCollection();
        private IServiceProvider autowiredProvider;
        private IServiceProvider baseProvider;
        private Stopwatch Watch { get; } = new ();

        [SetUp]
        public void Setup()
        {
            baseProvider = collection
                .AddSingleton<IA, A>()
                .AddSingleton<IA, A>()
                .AddSingleton<IA, A>()
                .AddScoped<IB, B>()
                .AddScoped<IC, C>()
                .AddTransient<ID, D>()
                .BuildServiceProvider();
            autowiredProvider = new ServiceCollection()
                .AddSingleton<IA, A>()
                .AddSingleton<IA, A>()
                .AddSingleton<IA, A>()
                .AddScoped<IB, B>()
                .AddScoped<IC, C>()
                .AddTransient<ID, D>()
                .BuildAutowiredServiceProvider(ServiceCollectionContainerBuilderExtensions.BuildServiceProvider);
            
        }
       
        [Test]
        public void TestNativeResolve()
        {
            baseProvider.GetRequiredService<IB>();
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = baseProvider.GetRequiredService<IB>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Native resolve cost {watch.ElapsedTicks}");
        }
        
        [Test]
        public void TestResolve()
        {
            autowiredProvider.GetRequiredService<IB>();
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = autowiredProvider.GetRequiredService<IB>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Autowired resolve cost {watch.ElapsedTicks}");
        }

        [Test]
        public void TestService()
        {
            var a1 = autowiredProvider.GetRequiredService<IA>();
            var c1 = autowiredProvider.GetRequiredService<IC>();
            var d1 = autowiredProvider.GetRequiredService<ID>();
            var d11 = autowiredProvider.GetRequiredService<ID>();
            
            var scope1 = autowiredProvider.CreateScope();
            var c2 = scope1.ServiceProvider.GetRequiredService<IC>();
            var d2 = scope1.ServiceProvider.GetRequiredService<ID>();
            
            var scope2 = autowiredProvider.CreateScope();
            var c3 = scope2.ServiceProvider.GetRequiredService<IC>();
            var d3 = scope2.ServiceProvider.GetRequiredService<ID>();
        }
        
        [Test]
        public void TestResolveNativeCollection()
        {
            var aS =  baseProvider.GetService<IEnumerable<IA>>()!;
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                aS =  baseProvider.GetService<IEnumerable<IA>>()!;
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Native collection cost {watch.ElapsedTicks}");
            Assert.That(aS.Count(), Is.GreaterThan(0));
            Assert.That(((A)aS.ElementAt(0)).B, Is.Not.Null);
        }
       
        [Test]
        public void TestResolveCollection()
        {
            var aS =  autowiredProvider.GetService<IEnumerable<IA>>()!;
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                aS =  autowiredProvider.GetService<IEnumerable<IA>>()!;
                times--;
            }
            watch.Stop();
            Console.WriteLine($"Autowired collection cost {watch.ElapsedTicks}");
            Assert.That(aS.Count(), Is.GreaterThan(0));
            Assert.That(((A)aS.ElementAt(0)).B, Is.Not.Null);
        }


        [Test]
        public void TestType()
        {
            var aS = autowiredProvider.GetService<IEnumerable<IA>>()!;
            var b = aS is IEnumerable<object>;
        }
    }
}