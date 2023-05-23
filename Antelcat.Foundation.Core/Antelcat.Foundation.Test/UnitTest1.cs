using System.Diagnostics;
using System.Reflection;
using Antelcat.Foundation.Core.Attributes;
using Antelcat.Foundation.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

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
                    .AddSingleton<IC, C>()
                    .AddSingleton<IB, B>()
                    .AddSingleton<IA, A>()
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
        public void TestService()
        {
            var b = provider.GetRequiredService<IB>();
            var b2 = provider.GetRequiredService<IB>();
        }

        public interface IA { }
        public class A : IA { }
        public interface IB { }
        public class B : IB {
            public B() { }

            [Autowired]
            private readonly IC C;
            [Autowired]
            private IA A { get; set; }
        }

        public interface IC { }

        public class C : IC
        {
            public C(IB b)
            {
                B = b;
            }

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
        [Test]
        public async Task Run()
        {
            
        }
    }
}