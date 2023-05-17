using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Feast.Foundation.Core.Extensions;
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
            collection.AddSingleton<IC, C>();
            collection.AddSingleton<IB, B>();
            collection.AddSingleton<IA, A>();
            provider = collection.BuildServiceProvider();
            
        }

        public interface IA { }
        public class A : IA { }
        public interface IB { }
        public class B : IB {
            public B(IA a)
            {
                
            }
        }

        public interface IC { }

        public class C : IC
        {
            public C(IB b)
            {
                
            }
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