using Antelcat.Foundation.Core.Attributes;
using Antelcat.Foundation.Core.Implements.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Antelcat.Foundation.Server.Filters;
using Antelcat.Foundation.Server.Implements;
using Antelcat.Foundation.Server.Utils;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Antelcat.Foundation.Server.Extensions
{
    public static partial class ServiceExtension
    {
        public static void ConfigureJwt<TIdentity>(
           this IServiceCollection services,
           Action<TokenValidationParameters>? configure = null,
           Func<TIdentity, Task>? validation = null,
           Func<JwtBearerChallengeContext, string>? failed = null)
        {
            var config = new JwtConfigure<TIdentity>(configure);
            services
                .AddSingleton(config)
                .AddAuthentication(static options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.IncludeErrorDetails = true;
                    o.TokenValidationParameters = config.Parameters;
                    o.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = validation == null
                            ? static _ => Task.CompletedTask
                            : static async context =>
                            {
                                var token = (context.SecurityToken as JwtSecurityToken)!.RawData;
                                if (JwtExtension<TIdentity>.FromToken(token) == null)
                                {
                                    context.Fail(
                                        new NullReferenceException($"Cannot resolve {typeof(TIdentity)} from token"));
                                }
                                await Task.CompletedTask;
                            },

                        OnChallenge = async context =>
                        {
                            if (failed == null) return;
                            context.HandleResponse();
                            context.Response.Clear();
                            context.Response.Headers.Clear();
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync(failed(context));
                        }
                    };
                });
        }

    }

    public static partial class ServiceExtension
    {
        public static IServiceCollection AddJwtSwaggerGen(this IServiceCollection collection)
            => collection.AddSwaggerGen(static o =>
            {
                o.OperationFilter<AuthorizationFilter>();
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt Token Format like [ Bearer {Token} ]",
                    Name = nameof(Authorization),
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

        /// <summary>
        /// 使用 <see cref="AutowiredServiceProviderFactory"/> 作为服务生成工厂， 
        /// 实现自动注入携带 <see cref="AutowiredAttribute"/> 注解的属性和字段
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseAutowiredServiceProviderFactory(this IHostBuilder builder) 
            => builder.UseServiceProviderFactory(new AutowiredServiceProviderFactory(
                ServiceCollectionContainerBuilderExtensions.BuildServiceProvider));

        /// <summary>
        /// 使用 <see cref="AutowiredServiceProviderFactory{TAttribute}"/> 作为服务生成工厂
        /// </summary>
        /// <typeparam name="TAttribute">属性</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseAutowiredServiceProviderFactory<TAttribute>(this IHostBuilder builder)
            where TAttribute : Attribute 
            => builder.UseServiceProviderFactory(new AutowiredServiceProviderFactory<TAttribute>(
                ServiceCollectionContainerBuilderExtensions.BuildServiceProvider));

        /// <summary>
        /// 将 <see cref="IControllerActivator"/> 的实现替换为 <see cref="AutowiredControllerActivator"/> ,
        /// 且应当在 <see cref="MvcCoreMvcBuilderExtensions.AddControllersAsServices"/> 之后调用
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IServiceCollection UseAutowiredControllers(this IMvcBuilder collection) 
            => collection.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, AutowiredControllerActivator>());
        
        /// <summary>
        /// 将 <see cref="IControllerActivator"/> 的实现替换为 <see cref="AutowiredControllerActivator{TAttribute}"/> ,
        /// 且应当在 <see cref="MvcCoreMvcBuilderExtensions.AddControllersAsServices"/> 之后调用
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static IServiceCollection UseAutowiredControllers<TAttribute>(this IMvcBuilder collection)
        where TAttribute : Attribute
            => collection.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, AutowiredControllerActivator<TAttribute>>());
    }
}
