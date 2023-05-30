using System.Diagnostics;
using Antelcat.Foundation.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Feast.Foundation.Test;


public class ServiceTest
{
    private IServiceProvider autowiredProvider;
    private IServiceProvider nativeProvider;
    private Stopwatch Watch { get; } = new();

    private const int Times = 1000;

    [SetUp]
    public void Setup()
    {
        var registry = (IServiceCollection c) => c
            .AddSingleton(typeof(IGeneric<>), typeof(GenericType<>))
            .AddSingleton(typeof(IMultiGeneric<,,>), typeof(MultiGenericType<,,>))
            .AddSingleton<IA, A>()
            .AddSingleton<IA, A>()
            .AddScoped<IB, B>()
            .AddScoped<IC, C>()
            .AddTransient<ID, D>();
        nativeProvider =
            registry(new ServiceCollection()).BuildServiceProvider();
        autowiredProvider =
            registry(new ServiceCollection()).BuildAutowiredServiceProvider(static x => x.BuildServiceProvider());

        CurrentTest = Singletons;
    }

    private Tuple<Action, Action> CurrentTest;
    [Test]
    public void TestNative()
    {
        CurrentTest.Item1();
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times-- > 0)  CurrentTest.Item1();
        watch.Stop();
        Console.WriteLine($"Native resolve cost {watch.ElapsedTicks}");
    }

    [Test]
    public void TestAutowired()
    {
        CurrentTest.Item2();
        var times = Times;
        var watch = new Stopwatch();
        watch.Start();
        while (times-- > 0)  CurrentTest.Item2();
        watch.Stop();
        Console.WriteLine($"Autowired resolve cost {watch.ElapsedTicks}");
    }
    
    [Test]
    public void TestServiceResolve()
    {
        typeof(Type).GetMethod().CreateInvoker()
        TestAutowired();
        TestNative();
    }

    private Tuple<Action, Action> Singletons => new (
        () => nativeProvider.GetService<IA>(),
        () => autowiredProvider.GetService<IA>()
    );

    private Tuple<Action, Action> Scopes => new (
        () => nativeProvider.GetService<IB>(),
        () => autowiredProvider.GetService<IB>()
    );

    private Tuple<Action, Action> Transients => new (
        () => nativeProvider.GetService<ID>(),
        () => autowiredProvider.GetService<ID>()
    );

    private Tuple<Action, Action> Generics => new (
        () => nativeProvider.GetService<IMultiGeneric<int, double, object>>(),
        () => autowiredProvider.GetService<IMultiGeneric<int, double, object>>()
    );

    private Tuple<Action, Action> Collections => new (
        () => nativeProvider.GetService<IEnumerable<IA>>(),
        () => autowiredProvider.GetService<IEnumerable<IA>>()
    );

    [Test]
    public void TestService()
    {
        var a1 = autowiredProvider.GetRequiredService<IA>();
        var c1 = autowiredProvider.GetRequiredService<IC>();
        var d1 = autowiredProvider.GetRequiredService<ID>();
        var d11 = autowiredProvider.GetRequiredService<ID>();

        var scope1 = autowiredProvider.CreateScope();
        var c2 = scope1.ServiceProvider.GetRequiredService<IC>();
        var d2 = scope1.ServiceProvider.GetRequiredService<ID>();

        var scope2 = autowiredProvider.CreateScope();
        var c3 = scope2.ServiceProvider.GetRequiredService<IC>();
        var d3 = scope2.ServiceProvider.GetRequiredService<ID>();
    }

    [Test]
    public void TestType()
    {
        var aS = autowiredProvider.GetService<IEnumerable<IA>>()!;
        var b = aS is IEnumerable<object>;
    }
}