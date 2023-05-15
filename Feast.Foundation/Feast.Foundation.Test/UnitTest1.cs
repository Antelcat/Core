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
    
        }


        private Task? Task;
        [Test]
        public async Task TestDirect()
        {
            Task = TestReflect();
        }

        private CancellationTokenSource? cancaler;
        [Test]
        public async Task TestReflect()
        {
            cancaler?.Cancel();
            Console.WriteLine("Start");
        }
        [Test]
        public void TestDelegate()
        {
         
        }
       
    }
}