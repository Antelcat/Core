# Antelcat.Foundation.Core

A series of dotnet codes

## Dependency-Injection

Extensions of native [.NET dependency injection](https://github.com/dotnet/docs/blob/main/docs/core/extensions/dependency-injection.md) with [Autowired](./Antelcat.Foundation.Core/Antelcat.Foundation.Core/Attributes/AutowiredAttribute.cs), provides a way to support properties and fields injection.

All lifetimes and generics are now supported. And using [IL delegates](#il-delegates) to speed up the setter.

``` c#
public class Service{
    [Autowired]
    private readonly IService dependency;
    [Autowired]
    private IService Dependency { get; set; }
} 
```

In common use :

``` c#
IServiceProvider provider = new ServiceCollection()
                            .Add(...)
                            .BuildAutowiredServiceProvider(static x=>x.BuildServiceProvider());
IService service = provider.GetService<IService>();
```

In [ASP.NET Core](https://github.com/dotnet/aspnetcore) :

```c#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers() //register controllers
                .AddControllersAsServices() // add controllers as services
                .UseAutowiredControllers(); // use auto wired controllers
builder.Host.UseAutowiredServiceProviderFactory(); // autowired services
```

Tests could be found in [ServiceTest.cs](./Antelcat.Foundation.Core/Antelcat.Foundation.Test/ServiceTest.cs) , which shows higher performance than Autofac and is close to native.

## IL Delegates

Along with [T4](https://learn.microsoft.com/zh-cn/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2022) generated [Emit Extension](./Antelcat.Foundation.Core/Antelcat.Foundation.Core/Extensions/ILExtension.g.cs)

+ Type (CreateCtor)
+ PropertyInfo (CreateSetter/CreateGetter)
+ FieldInfo (CreateSetter/CreateGetter)
+ MethodInfo (CreateInvoker)
  
Can be easily create delegate like :

``` c#
Invoker<object,object> invoker = typeof(Program).GetMethod("Main").CreateInvoker();
invoker.Invoke(null, new string[]{});
```