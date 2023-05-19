using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Server.Extensions;
using Feast.Foundation.Server.Test.Models;

namespace Feast.Foundation.Server.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAntelcatLogger();
            // Add services to the container.
            builder.Services
                .AddControllers()
                .AddControllersAsServices()
                .UseAutowiredControllers();
            builder.Services.ConfigureJwt<User>(
                failed: static _ => @"{ ""?"" : ""?"" }");
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddJwtSwaggerGen();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}