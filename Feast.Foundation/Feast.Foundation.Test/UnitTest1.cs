using System.Dynamic;
using System.Reflection;

namespace Feast.Foundation.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            var proxy = DispatchProxy.Create<ExpandoObject,DispatchProxy>();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}