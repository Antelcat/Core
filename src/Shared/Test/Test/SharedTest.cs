using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using Antelcat.Exceptions;
using Antelcat.Extensions;
using Antelcat.Implements;
using Antelcat.Implements.Converters;
using NUnit.Framework;

namespace Antelcat.Shared.Test;

public class RefClass{}
public class TestClass
{
    public RefClass RefProp { get; set; } = new();
    public RefClass RefField = new();

    public int ValueProp { get; set; } = 0;
    public int ValueField = 0;
    
    public static RefClass StaticRefProp { get; set; } = new();
    public static RefClass StaticRefField = new();

    public static int StaticValueProp { get; set; } = 0;
    public static int StaticValueField = 0;

}

class SharedTest
{
    private TypeConverter Converter;
    [SetUp]
    public void Setup()
    {
        Converter = new StringToFloatConverter();
    }

    [Test]
    public void Test()
    {
        var type = typeof(TestClass);
        var Prefix = "Value";
        var prop = type.GetProperty($"{Prefix}Prop")!;
        var field = type.GetField($"{Prefix}Field")!;

        var i = 1;
        var valueGetter = () => i++;
        TestIL((TestClass)new TestClass(), prop, field, () => i++);
        
        Debugger.Break();
    }

    public void TestIL<TTarget, TValue>(TTarget instance, PropertyInfo prop, FieldInfo field, Func<TValue> valueGetter)
    {
        var objInst = (object)instance;
        prop.CreateSetter<TTarget, object>().Invoke(ref instance,  valueGetter());
        var val2 = prop.CreateGetter<TTarget, object>().Invoke(instance);
        prop.CreateSetter<TTarget, TValue>().Invoke(ref instance, valueGetter());
        var val1 = prop.CreateGetter<TTarget, TValue>().Invoke(instance);
        prop.CreateSetter<object, TValue>().Invoke(ref objInst,  valueGetter());
        var val3 = prop.CreateGetter<object, TValue>().Invoke(instance);
        prop.CreateSetter<object, object>().Invoke(ref objInst,  valueGetter());
        var val4 = prop.CreateGetter<object, object>().Invoke(instance);
        
        
        field.CreateSetter<TTarget, TValue>().Invoke(ref instance,  valueGetter());
        var val5 = field.CreateGetter<TTarget, TValue>().Invoke(instance);
        field.CreateSetter<TTarget, object>().Invoke(ref instance,  valueGetter());
        var val6 = field.CreateGetter<TTarget, object>().Invoke(instance);
        field.CreateSetter<object, TValue>().Invoke(ref objInst,  valueGetter());
        var val7 = field.CreateGetter<object, TValue>().Invoke(instance);
        field.CreateSetter<object, object>().Invoke(ref objInst,  valueGetter());
        var val8 = field.CreateGetter<object, object>().Invoke(instance);
        Debugger.Break();
    }


    private const int Times = 1000;

    [Test]
    public void TestInline()
    {
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times -- > 0)
        {
            "123".Inline(1, "", 3f);
        }
        watch.Stop();
        Console.WriteLine($"Inline   Cost : {watch.ElapsedTicks}");
            
        times = Times;
        watch = new Stopwatch();
        watch.Start();
        while (times -- > 0)
        {
            "123".NoInline(1, "", 3f);
        }
        watch.Stop();
        Console.WriteLine($"Noinline Cost : {watch.ElapsedTicks}");
            
        Console.WriteLine();
    }

    [Test]
    public async Task MultipleTest()
    {
        var i = 0;
        while (i++ < 3)
        {
            await 1000;
            Console.WriteLine($"预热{i}s");
        }

        var times = 5;
        while (times-- > 0)
        {
            TestInline();
        }
    }

    
    [Test]
    public async Task TestTask()
    {
        int Fun() => 3;
        var i = await (Func<int>)Fun;
    }

    [Test]
    public void TestCompare()
    {
        if (new Dictionary<string, string>
            {
                { "123", "sss" }
            }.DeepEquals(new Dictionary<string, string>
            {
                { "123", "sss" }
            }))
        {
            Debugger.Break();
        }
    }

    delegate uint H();

    delegate void GetDelegateForFunctionPointerInternal();
    [Test]
    public unsafe void TestNative()
    {
        var handler = new FileInfo(
                @"D:\Shared\WorkSpace\Git\libuv-sharp\LibuvSharp\LibuvSharp.Test\bin\Debug\net8.0\runtimes\win-x64\native\libuv.dll")
            .GetFunctionDelegate<Func<nint>>("uv_version");
        var version = handler.Invoke();
    }
    
    [Test]
    public void TestEnumerable()
    {
        var list = new List<string>();
        for(var i = 0 ; i< 100 ; i++){
            list.Add($"{i}");
        }
        foreach(var item in list.WithIndex()){
            Console.WriteLine(item);
        }
    }

    [Test]
    public void TestException()
    {
        try
        {
            throw new StackTraceException();
        }
        catch (StackTraceException ex)
        {
            Debugger.Break();
        }
    }
}

public static class Extension
{
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void NoInline<T>(this T instance, int i1, string i2, float i3)
    {
        i3 = i2.Length + i1;
        new StackTrace().GetFrames();
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Inline<T>(this T instance, int i1, string i2, float i3)
    {
        i3 = i2.Length + i1;
        new StackTrace().GetFrames();
    }
}