using System.Diagnostics;
using System.Reflection;
using Antelcat.Core.Structs.IL;
using Antelcat.Extensions;
using Antelcat.Core.Extensions;

namespace Antelcat.Core.Test;

public class TestIl
{
    public class Example
    {
        public string Property { get; set; } = "name";

        public static int NormalMethod(int count)
        {
            return count;
        }
        public string Method(out int count)
        {
            count = 1;
            return Property;
        }

        public static string StaticMethod(string input, out int count)
        {
            count = 2;
            return input;
        }
    }
    
    [SetUp]
    public void SetUp()
    {
        PropertyInfo = typeof(Example).GetProperty(nameof(Example.Property))!;
        Getter = (ILGetter)PropertyInfo;
    }

    private PropertyInfo PropertyInfo;
    private ILGetter Getter;
    private Invoker<object,object> Method = typeof(Example).GetMethod(nameof(Example.Method))!.CreateInvoker();
    private readonly Example example = new();
    int Times = 10000;

    [Test]
    public void TestOutIlInvoke()
    {
        var args = new object?[] { null };
        var ret = Method.Invoke(example, args);
    }

    [Test]
    public void TestIlInvoke()
    {
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times-- > 0)
        {
            Getter.Getter.Invoke(example);
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
            PropertyInfo.GetValue(example);
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
            PropertyInfo.SetValue(example,"name");
        }
        watch.Stop();
        Console.WriteLine($"Setter cost {watch.ElapsedTicks}");
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
            pointer = MarshalExtension.ToPointer(bytes);
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