using System.Diagnostics;
using System.Reflection;
using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Core.Structs.IL;

namespace Feast.Foundation.Test;

public class TestIl
{
    public class Tmp
    {
        public string Name { get; set; } = "name";
    }
    
    [SetUp]
    public void SetUp()
    {
        PropertyInfo = typeof(Tmp).GetProperty("Name")!;
        Getter = (ILGetter)PropertyInfo;
    }

    private PropertyInfo PropertyInfo;
    private ILGetter Getter;
    private readonly Tmp tmp = new();
    int Times = 10000;
    
    [Test]
    public void TestIlInvoke()
    {
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times-- > 0)
        {
            Getter.Getter.Invoke(tmp);
        }
        watch.Stop();
        Console.WriteLine($"IlInvoke cost {watch.ElapsedTicks}");
    }

    [Test]
    public void TestGetter()
    {
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times-- > 0)
        {
            PropertyInfo.GetValue(tmp);
        }
        watch.Stop();
        Console.WriteLine($"Getter cost {watch.ElapsedTicks}");
    }
    
    [Test]
    public void TestSetter()
    {
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times-- > 0)
        {
            PropertyInfo.SetValue(tmp,"name");
        }
        watch.Stop();
        Console.WriteLine($"Getter cost {watch.ElapsedTicks}");
    }

    private byte[] bytes = { 1, 2, 3, 4, 5 };
    [Test]
    public unsafe void TestPointer()
    {
        byte* pointer = null;
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times-- > 0)
        {
            pointer = bytes.ToPointer();
        }
        watch.Stop();
        for (var i = 0; i < bytes.Length; i++)
        {
            Console.Write(*pointer);
            pointer++;
        }
        Console.WriteLine();
        Console.WriteLine($"Fixed cost {watch.ElapsedTicks}");
    }

    [Test]
    public unsafe void TestUnsafe()
    {
        byte* pointer = null;
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times-- > 0)
        {
            pointer = (byte*)bytes.ToIntPtr();
        }
        watch.Stop();
        for (var i = 0; i < bytes.Length; i++)
        {
            Console.Write(*pointer);
            pointer++;
        }
        Console.WriteLine();
        Console.WriteLine($"IntPtr cost {watch.ElapsedTicks}");
    }
}