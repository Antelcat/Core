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
                .BuildAutowiredServiceProvider(c => c.BuildServiceProvider());
        }


        public class Tmp
        {
            [PathArg]
            public string App { get; set; } = "MyApp";
            [PathArg]
            public string ProgramFiles { get; set; } = "D:ProgramFiles";
            [PathArg]
            public string DatabaseDir { get; set; } = "[ProgramFiles]/[App]/db";
            [PathArg]
            public string DatabaseFile { get; set; } = "[DatabaseDir]/file.db";
        }

        public class Boolable
        {
            public bool Bool { get; set; } = true;
        }
        [Test]
        public void TestPath()
        {
            var i = typeof(Boolable).GetProperties()[0].CreateGetter();
            var t = (object)new Boolable();
            var r = i.Invoke(t);
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
            var bb = provider.GetRequiredService<IB>();
            var scope = provider.CreateScope();
            var bbbb = scope.ServiceProvider.GetRequiredService<IB>();
            scope = provider.CreateScope();
            var b = scope.ServiceProvider.GetRequiredService<IB>();
            var c = scope.ServiceProvider.GetRequiredService<IC>();
            var b2 = scope.ServiceProvider.GetRequiredService<IB>();
            var c2 = scope.ServiceProvider.GetRequiredService<IC>();
        }

        public interface IA { }

        public class A : IA
        {
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
            [Autowired]
            private readonly IB B;
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