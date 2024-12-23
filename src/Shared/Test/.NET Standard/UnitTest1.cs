using Antelcat.Extensions;

namespace Antelcat.Shared.Test.NET_Standard
{
    public class TestClass
    {
        public TestClass(int i)
        {
            
        }
    }
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var  ctor     = typeof(TestClass).GetConstructor(new[] { typeof(int) });
            var  del      = ctor!.CreateCtor();
            var  instance = del.Invoke(1);
            Assert.Pass();          
        }
    }
}