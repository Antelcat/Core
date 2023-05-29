using System.Diagnostics;
using Antelcat.Foundation.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Feast.Foundation.Test
{
    public class Tests
    {
        readonly IServiceCollection collection = new ServiceCollection();
        private IServiceProvider autowiredProvider;
        private IServiceProvider nativeProvider;
        private Stopwatch Watch { get; } = new ();

        private const int Times = 1000;
        [SetUp]
        public void Setup()
        {
            nativeProvider = collection
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
        public void TestResolveNativeSingleton()
        {
            nativeProvider.GetRequiredService<IA>();
            var times = Times;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = nativeProvider.GetRequiredService<IA>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Native singleton resolve cost {watch.ElapsedTicks}");
        }
        [Test]
        public void TestResolveAutowiredSingleton()
        {
            autowiredProvider.GetRequiredService<IA>();
            var times = Times;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = autowiredProvider.GetRequiredService<IA>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Autowired singleton resolve cost {watch.ElapsedTicks}");
        }

        [Test]
        public void TestResolveNativeScoped()
        {
            nativeProvider.GetRequiredService<IB>();
            var times = Times;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = nativeProvider.GetRequiredService<IB>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Native scoped resolve cost {watch.ElapsedTicks}");
        }
        [Test]
        public void TestResolveAutowiredScoped()
        {
            autowiredProvider.GetRequiredService<IB>();
            var times = Times;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = autowiredProvider.GetRequiredService<IB>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Autowired scoped resolve cost {watch.ElapsedTicks}");
        }
        
        [Test]
        public void TestResolveNativeTransient()
        {
            nativeProvider.GetRequiredService<ID>();
            var times = Times;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = nativeProvider.GetRequiredService<ID>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Native transient resolve cost {watch.ElapsedTicks}");
        }
        [Test]
        public void TestResolveAutowiredTransient()
        {
            autowiredProvider.GetRequiredService<ID>();
            var times = Times;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = autowiredProvider.GetRequiredService<ID>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Autowired transient resolve cost {watch.ElapsedTicks}");
        }
        
        [Test]
        public void TestResolveNativeCollection()
        {
            var aS =  nativeProvider.GetService<IEnumerable<IA>>()!;
            var times = Times;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                aS =  nativeProvider.GetService<IEnumerable<IA>>()!;
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
            var times = Times;
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
        public void TestType()
        {
            var aS = autowiredProvider.GetService<IEnumerable<IA>>()!;
            var b = aS is IEnumerable<object>;
        }
    }
}