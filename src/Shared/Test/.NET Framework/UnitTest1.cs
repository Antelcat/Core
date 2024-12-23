using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Antelcat.Extensions;
using Antelcat.Implements;
using NUnit.Framework;

namespace Antelcat.Shared.Test.NET_Framework
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            ((Action)(() => { })).Run();
            new Action(() => { }).Run();
        }

        delegate uint Handler();
        [Test]
        public void Test1()
        {
           

            Console.WriteLine();
        }
    }
}