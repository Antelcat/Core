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
            var method = typeof(StringExtension)
                             .GetMethods(BindingFlags.Static | BindingFlags.Public)
                             .FirstOrDefault(x =>
                                 x.GetParameters().Length == 1
                                 && x.GetParameters()[0].ParameterType == typeof(string)
                                 && x.Name.StartsWith("To")
                                 && x.ReturnType == typeof(int))
                         ?? throw new NotSupportedException(
                             $"{nameof(Int32)} convert from {nameof(String)} was not supported");
            DirectMethod = s => ((string)s).ToInt();
            ReflectMethod = s => method.Invoke(null, new[] { s })!;
            var invoker = method.CreateInvoker();
            DelegateMethod = s => invoker.Invoke(null, new[] { s })!;
        }

        Func<object, object> DirectMethod;
        Func<object, object> ReflectMethod;
        Func<object, object> DelegateMethod;

        [Test]
        public void TestDirect()
        {
            object res = null!;
            Watch.Start();
            for (var i = 0; i < 10000; i++)
            {
                res = DirectMethod.Invoke("1");
            }
            Watch.Stop();
            Console.WriteLine($"Result is {res}");
            Console.WriteLine(Watch.ElapsedTicks);
        }

        [Test]
        public void TestReflect()
        {
            object res = null!;
            Watch.Start();
            for (var i = 0; i < 10000; i++)
            {
                res = ReflectMethod.Invoke("1");
            }
            Watch.Stop();
            Console.WriteLine($"Result is {res}");
            Console.WriteLine(Watch.ElapsedTicks);
        }
        [Test]
        public void TestDelegate()
        {
            object res = null!;
            Watch.Start();
            for (var i = 0; i < 10000; i++)
            {
                res = DelegateMethod.Invoke("1");
            }
            Watch.Stop();
            Console.WriteLine($"Result is {res}");
            Console.WriteLine(Watch.ElapsedTicks);
        }
       
    }
}