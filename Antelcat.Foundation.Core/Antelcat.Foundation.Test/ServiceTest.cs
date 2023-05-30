using System.Diagnostics;
using Antelcat.Foundation.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Feast.Foundation.Test;


public class ServiceTest
{
    readonly IServiceCollection collection = new ServiceCollection();
    private IServiceProvider autowiredProvider;
    private IServiceProvider nativeProvider;
    private Stopwatch Watch { get; } = new();

    private const int Times = 1000;

    [SetUp]
    public void Setup()
    {
        collection
            .AddSingleton(typeof(IGeneric<>), typeof(GenericType<>))
            .AddSingleton(typeof(IMultiGeneric<,,>), typeof(MultiGenericType<,,>))
            .AddSingleton<IA, A>()
            .AddSingleton<IA, A>()
            .AddScoped<IB, B>()
            .AddScoped<IC, C>()
            .AddTransient<ID, D>();
        nativeProvider =
            collection.BuildServiceProvider();
        autowiredProvider =
            collection.BuildAutowiredServiceProvider(ServiceCollectionContainerBuilderExtensions.BuildServiceProvider);

        NativeTest = NativeSingleton;
        AutowiredTest = AutowiredSingleton;
        NativeTest();
        AutowiredTest();
    }

    private Action NativeTest;
    private Action AutowiredTest;
    
    [Test]
    public void TestServiceResolve()
    {
        var times1 = Times;
        var watch1 = new Stopwatch();
        watch1.Start();
        while (times1-- > 0) NativeTest();
        watch1.Stop();
        Console.WriteLine($"Native resolve cost {watch1.ElapsedTicks}");

        var times2 = Times;
        var watch2 = new Stopwatch();
        watch2.Start();
        while (times2-- > 0) AutowiredTest();
        watch2.Stop();
        Console.WriteLine($"Autowired resolve cost {watch2.ElapsedTicks}");
    }

    
    
    public Action NativeSingleton => () => nativeProvider.GetService<IA>();
    public Action AutowiredSingleton => () => autowiredProvider.GetService<IA>();
    public Action NativeScoped => () => nativeProvider.GetService<IB>();
    public Action AutowiredScoped => () => autowiredProvider.GetService<IB>();
    public Action NativeTransient => () => nativeProvider.GetService<ID>();
    public Action AutowiredTransient => () => autowiredProvider.GetService<ID>();
    public Action NativeGeneric => () => nativeProvider.GetService<IMultiGeneric<int, double, object>>();
    public Action AutowiredGeneric => () => autowiredProvider.GetService<IMultiGeneric<int, double, object>>();
    public Action NativeCollection => () => nativeProvider.GetService<IEnumerable<IA>>();
    public Action AutowiredCollection => () => autowiredProvider.GetService<IEnumerable<IA>>();

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