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
            collection.ForEach(x=>x.ServiceType);
        }


        public async Task LongRun(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Console.WriteLine($"Running{DateTime.Now:yyyy-MM-dd-HH-mm-ss}");
                await 500;
            }
        }

        [Test]
        public async Task Run()
        {
            var source = new CancellationTokenSource();
            Task.Run(async () =>
            {
                await 4000;
                source.Cancel();
            }).Detach();
            try
            {
                await LongRun(source.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Assert.NotNull(e);
            }
            finally
            {
                Console.WriteLine("Canceled");
            }
        }
    }
}