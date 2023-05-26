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
        private IServiceProvider provider;
        private IServiceProvider baseProvider;
        private Stopwatch Watch { get; } = new ();

        [SetUp]
        public void Setup()
        {
            baseProvider = new ServiceCollection()
                .AddSingleton<IA, A>()
                .AddScoped<IB, B>()
                .AddScoped<IC, C>()
                .AddTransient<ID, D>()
                .BuildServiceProvider();
            provider = collection
                .AddSingleton<IA, A>()
                .AddScoped<IB, B>()
                .AddScoped<IC, C>()
                .AddTransient<ID, D>()
                .BuildAutowiredServiceProvider(ServiceCollectionContainerBuilderExtensions.BuildServiceProvider);
            Dictionary = new()
            {
                { typeof(IA), 1 },
                { typeof(IB), 2 },
                { typeof(IC), 3 }
            };
        }

        private Dictionary<Type, int> Dictionary = new Dictionary<Type, int>();
        
        [Test]
        public void TestGet()
        {
            var type = typeof(IC);
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = Dictionary[type];
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Get resolve cost {watch.ElapsedTicks}");
        }

        [Test]
        public void TestTryGet()
        {
            var type = typeof(IC);
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = Dictionary.TryGetValue(type, out var s);
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Try Get resolve cost {watch.ElapsedTicks}");
        }
        
        [Test]
        public void TestNativeResolve()
        {
            baseProvider.GetRequiredService<ID>();
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = baseProvider.GetRequiredService<ID>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Native resolve cost {watch.ElapsedTicks}");
        }
        
        [Test]
        public void TestResolve()
        {
            provider.GetRequiredService<ID>();
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = provider.GetRequiredService<ID>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Autowired resolve cost {watch.ElapsedTicks}");
        }

        [Test]
        public void TestService()
        {
            var a1 = provider.GetRequiredService<IA>();
            var c1 = provider.GetRequiredService<IC>();
            var d1 = provider.GetRequiredService<ID>();
            var d11 = provider.GetRequiredService<ID>();
            
            var scope1 = provider.CreateScope();
            var c2 = scope1.ServiceProvider.GetRequiredService<IC>();
            var d2 = scope1.ServiceProvider.GetRequiredService<ID>();
            
            var scope2 = provider.CreateScope();
            var c3 = scope2.ServiceProvider.GetRequiredService<IC>();
            var d3 = scope2.ServiceProvider.GetRequiredService<ID>();
        }
     
        [Test]
        public void RunSync()
        {
            bool Find(out int i)
            {
                try
                {
                    i = 1;
                    return true;
                }
                finally
                {
                    i = 9;
                    Console.WriteLine("Wait a minute");
                }
            }
            Console.WriteLine(Find(out var res) ? res : -1);
        }

        [Test]
        public void TestMarshal()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7 };
            var ptr = bytes.ToIntPtr();
            ptr.Dispose();
        }
    }
}