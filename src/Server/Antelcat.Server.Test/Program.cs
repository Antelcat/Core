using Antelcat.Core.Extensions;
using Antelcat.Core.Implements.Loggers;
using Antelcat.Extensions;
using Antelcat.Server.Extensions;
using Antelcat.Server.Test.Hubs;
using Antelcat.Server.Test.Models;
using Antelcat.Server.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntelcatLogger(static _ => new LoggerConfig().WithLogLevel(LogLevel.Error), true);

builder.Services
    .AddControllers()
    .AddAntelcatFilters(static _ => { })
    .AddControllersAsServices();

builder.Services
    .ConfigureSharedCookie(
        configure: cookie =>
        {
            cookie.HttpOnly = true;
            cookie.Name     = $"{nameof(Antelcat)}_{nameof(Antelcat.Server)}";
            cookie.OnDeniedJson(async _ => (HttpPayload)"权限不足")
                .OnFailedJson(async _ => (HttpPayload)"未授权");
        })
    .ConfigureJwt(
        configure: jwt =>
        {
            jwt.Secret      = Guid.NewGuid().ToString();
            jwt.OnValidated = static _ => Task.CompletedTask;
            jwt.OnForbidden = static _ => ((HttpPayload)"权限不足").Serialize();
            jwt.OnFailed    = static _ => ((HttpPayload)"未授权").Serialize();
        });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtSwaggerGen();
builder.Services.AddSignalR();
builder.Host.UseAutowiredServiceProviderFactory();

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
app.MapHub<StreamHub>("/stream");
app.Run();