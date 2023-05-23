using System.Diagnostics;
using Antelcat.Foundation.Core.Attributes;
using Antelcat.Foundation.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Feast.Foundation.Test
{
    public class Tests
    {
        IServiceCollection collection = new ServiceCollection();
        private IServiceProvider provider;
        private Stopwatch Watch { get; } = new ();

        [SetUp]
        public void Setup()
        {
            provider = collection
                .AddSingleton<IA, A>()
                .AddSingleton<IB, B>()
                .AddScoped<IC, C>()
                .AddTransient<ID, D>()
                .BuildAutowiredServiceProvider(ServiceCollectionContainerBuilderExtensions.BuildServiceProvider);
        }

        [Test]
        public void TestResolve()
        {
            provider.GetRequiredService<IC>();
            var times = 1000;
            var watch = new Stopwatch();
            watch.Start();
            while (times > 0)
            {
                var c = provider.GetRequiredService<IC>();
                times--;
            }

            watch.Stop();
            Console.WriteLine($"Autowired resolve cost {watch.ElapsedTicks}");
        }

        [Test]
        public void TestService()
        {
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

        public interface IA { }
        public class A : IA
        {
            private static int Count = 0;
            private readonly int Number = ++Count;
            [Autowired]
            private IB B { get; set; }
        }
        public interface IB { }
        public class B : IB
        {
            private static int Count = 0;
            private readonly int Number = ++Count;
            public B() { }
            [Autowired]
            private IA A { get; set; }
        }
        public interface IC { }
        public class C : IC
        {
            private static int Count = 0;
            private readonly int Number = ++Count;
            [Autowired]
            private readonly IA A;
            [Autowired]
            private readonly IB B;
        }
        public interface ID { }
        public class D : ID
        {
            private static int Count = 0;
            private readonly int Number = ++Count;
            [Autowired]
            private readonly IA A;
            [Autowired]
            private readonly IB B;
            [Autowired]
            private readonly IC C;
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
        
    }
}