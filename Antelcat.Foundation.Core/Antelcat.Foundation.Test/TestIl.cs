using System.Diagnostics;
using System.Reflection;
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
        while (times > 0)
        {
            Getter.Getter.Invoke(tmp);
            times--;
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
        while (times > 0)
        {
            PropertyInfo.GetValue(tmp);
            times--;
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
        while (times > 0)
        {
            PropertyInfo.SetValue(tmp,"name");
            times--;
        }
        watch.Stop();
        Console.WriteLine($"Getter cost {watch.ElapsedTicks}");
    }
}