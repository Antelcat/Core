using Microsoft.Extensions.DependencyInjection;

namespace Feast.Foundation.Test
{
    public class Tests
    {
        public interface IDemo
        {
            string Method();
        }

        public class Demo : IDemo
        {
            private readonly string message = "This is a demo";
            public Demo(object iNeedSomething) { }
            public string Method()
            {
                Console.WriteLine(message);
                return message;
            }
        }


        IServiceCollection collection = new ServiceCollection();
        private IServiceProvider provider;
        [SetUp]
        public void Setup()
        {
            collection.AddTransient<IDemo, Demo>();
            provider = collection.BuildServiceProvider();
        }

        [Test]
        public void Test1()
        {
            provider.GetRequiredService<IDemo>().Method();
            Assert.Pass();
        }
    }
}